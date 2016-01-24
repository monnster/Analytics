using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Analytics.Core.Data;
using Analytics.Core.Data.Entities;
using Analytics.Core.Data.Enums;
using Analytics.Core.Services.Interfaces;
using LinqToDB;
using NLog;

namespace Analytics.Core.Services.Impl
{
	public class ParserService: IParserService
	{
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		public BulkMaterialParseResult ParseMaterialPricesBulk(
			int manufacturerId,
			int supplierId,
			DateTime date,
			AlloyType alloyType,
			RollType rollType,
			bool remove,
			string raw)
		{
			var materials = new List<RawMaterial>();
			var errors = new List<string>();

			try
			{
				using (var storage = new Storage())
				{
					if (remove)
					{
						var allThickness = storage.RawMaterials
							.Where(
								rm => rm.ManufacturerId == supplierId
									&& (alloyType == AlloyType.Undefined || rm.RawMaterialType.AlloyType == alloyType)
									&& (rollType == RollType.Undefined || rm.RawMaterialType.RollType == rollType)
							)
							.Select(rm => rm.RawMaterialType.Thickness.ToString(CultureInfo.InvariantCulture))
							.ToList();

						raw = string.Join("\t", allThickness)
							+ "\n"
							+ string.Join("\t", allThickness.Select(p => ""));
					}

					var lines = raw.Split('\n');
					var headers = lines[0].Split('\t');
					var prices = lines[1].Split('\t');

					for (int i = 0; i < headers.Length; i++)
					{
						var thickness = Convert.ToDecimal(headers[i].FixDecimalSeparator());
						if (!string.IsNullOrEmpty(prices[i]))
						{
							var rawMaterial = storage
								.RawMaterials
								.Where(
									rm => rm.ManufacturerId == supplierId
										&& rm.RawMaterialType.AlloyType == alloyType
										&& rm.RawMaterialType.RollType == rollType
										&& rm.RawMaterialType.Thickness == thickness)
								.ToList();

							if (!rawMaterial.Any())
							{
								errors.Add($"Выбранный поставщик не производит материала с такими параметрами и толщиной {thickness}");
								continue;
							}
							if (rawMaterial.Count > 1)
							{
								errors.Add(
									$"Выбранный поставщик производит более 1 материала с такими параметрами и толщиной {thickness}, невозможно определить нужный.");
								continue;
							}

							var priceItem = new PriceItem
							{
								RawMaterialId = rawMaterial[0].RawMaterialId,
								OwnerId = manufacturerId,
								Date = date,
								Price = remove 
									? null 
									: (decimal?)Convert.ToDecimal(prices[i].FixDecimalSeparator())
							};

							rawMaterial[0].PriceItems = new[]
							{
								priceItem
							};

							materials.Add(rawMaterial[0]);
						}
					}
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				errors.Add("Ошибка синтаксического разбора строки.");
			}

			return new BulkMaterialParseResult
			{
				Errors = errors.ToArray(),
				Materials = errors.Any() 
					? new RawMaterial[0]
					: materials.ToArray()
			};
		}

		public BulkProductParseResult ParseExtraPricesBulk(
			int manufacturerId,
			int supplierId,
			DateTime date,
			AlloyType alloyType,
			RollType rollType,
			int priceExtraCategoryId,
			bool remove,
			string raw)
		{
			var products = new List<Product>();
			var errors = new List<string>();

			if (remove)
			{
				raw = "0";
			}

			try
			{
				var lines = raw.Split(new [] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
				var headers = lines[0].Split('\t');

				
				using (var storage = new Storage())
				{
					if (lines.Length == 1 && headers.Length == 1)
					{
						var price = Convert.ToDecimal(raw.Trim().FixDecimalSeparator());

						return new BulkProductParseResult
						{
							Errors = new string[0],
							Products = storage
								.Products
								.Where(
									p =>
										p.ManufacturerId == manufacturerId
											&& p.RawMaterial.ManufacturerId == supplierId
											&& (alloyType == AlloyType.Undefined || p.RawMaterial.RawMaterialType.AlloyType == alloyType)
											&& (rollType == RollType.Undefined || p.RawMaterial.RawMaterialType.RollType == rollType)
								)
								.ToList()
								.Select(
									p =>
									{
										p.PriceExtras = new[]
										{
											new PriceExtra
											{
												PriceExtraCategoryId = priceExtraCategoryId,
												PriceItems = new[]
												{
													new PriceItem
													{
														Date = date,
														OwnerId = manufacturerId,
														Price = price,
													}
												}
											}
										};
										return p;
									})
								.ToArray()
						};
					}


					for (int lineId = 1; lineId < lines.Length; lineId++)
					{
						var prices = lines[lineId].Split('\t');
						var productName = prices[0].Trim();

						for (int colId = 1; colId < headers.Length; colId++)
						{
							var thickness = Convert.ToDecimal(headers[colId].FixDecimalSeparator());

							var query = storage
									.RawMaterials
									.LoadWith(rm => rm.RawMaterialType)
									.Where(
										rm => rm.ManufacturerId == supplierId
											&& rm.RawMaterialType.Thickness == thickness);

							if (alloyType != AlloyType.Undefined)
							{
								query = query.Where(rm => rm.RawMaterialType.AlloyType == alloyType);
							}

							if (rollType != RollType.Undefined)
							{
								query = query.Where(rm => rm.RawMaterialType.RollType == rollType);
							}

							var rawMaterials = query.ToList();

							if (!rawMaterials.Any())
							{
								errors.Add($"Выбранный поставщик не производит материала с такими параметрами и толщиной {thickness}");
								continue;
							}

							foreach (var rawMaterial in rawMaterials)
							{
								var product = storage
									.Products
									.SingleOrDefault(
										p => p.ManufacturerId == manufacturerId
											&& p.RawMaterialId == rawMaterial.RawMaterialId
											&& p.Name == productName);

								if (null == product)
								{
									product = new Product
									{
										ManufacturerId = manufacturerId,
										RawMaterialId = rawMaterial.RawMaterialId,
										Thickness = rawMaterial.RawMaterialType.Thickness,
										Name = productName,
									};
								}

								var priceExtra = new PriceExtra
								{
									PriceExtraCategoryId = priceExtraCategoryId,
									ProductId = product.ProductId,
								};

								var price = string.IsNullOrEmpty(prices[colId])
									? null
									: (decimal?)Convert.ToDecimal(prices[colId].FixDecimalSeparator());

								var priceItem = new PriceItem
								{
									OwnerId = manufacturerId,
									Date = date,
									Price = remove ? null : price,
								};

								priceExtra.PriceItems = new[]
								{
									priceItem
								};

								product.PriceExtras = new[]
								{
									priceExtra
								};

								products.Add(product);


								if (!string.IsNullOrEmpty(prices[colId]))
							{
								
								}

							}
						}
					}
					
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				errors.Add("Ошибка синтаксического разбора строки.");
			}

			return new BulkProductParseResult
			{
				Errors = errors.ToArray(),
				Products = errors.Any()
					? new Product[0]
					: products.ToArray()
			};
		}
	}
}