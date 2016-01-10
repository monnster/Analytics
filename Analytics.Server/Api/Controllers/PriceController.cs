using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Analytics.Core;
using Analytics.Core.Data;
using Analytics.Core.Data.Entities;
using Analytics.Core.Data.Enums;
using Analytics.Core.Services.Interfaces;
using Analytics.Server.Api.Models;
using LinqToDB;

namespace Analytics.Server.Api.Controllers
{
	public class PriceController: ApiControllerBase
	{
		public IParserService ParserService { get; set; }

		public IProductService ProductService { get; set; }

		[Route("api/price/store-prices")]
		[HttpPost]
		public void StorePrices([FromBody] BulkPriceAddModel model)
		{
			Argument.NotNull(model, "Model is required.");

			model.Date = model.Date.Date;

			switch (model.PriceType)
			{
				case PriceType.PriceExtra:
					StoreProductPrices(model);
					break;

				case PriceType.RawMaterial:
					StoreMaterialPrices(model);
					break;
			}
		}

		[Route("api/price/get-price-list")]
		[HttpPost]
		public PricelistModel GetPricelist([FromBody] PriceFilterModel filter)
		{
			Argument.NotNull(filter, "Pricelist filter is required.");

			var productNames = new List<string>();
			var productPrices = new List<decimal[]>();

			using (var storage = new Storage())
			{
				var products = storage
					.Products
					.Where(
						p => p.ManufacturerId == filter.ManufacturerId
							&& p.RawMaterial.RawMaterialType.AlloyType == filter.AlloyType
							&& p.RawMaterial.RawMaterialType.RollType == filter.RollType)
					.Select(
						p => new
						{
							ProductId = p.ProductId,
							Name = p.Name,
							Thickness = p.Thickness,
							Price = 0,
						})
					.ToList();

				var colNames = products
					.Select(p => p.Thickness)
					.Distinct()
					.OrderBy(p => p)
					.ToArray();

				foreach (var row in products.GroupBy(p => p.Name))
				{
					productNames.Add(row.Key);
					var rowPrices = colNames
						.Select(
							thickness =>
							{
								var product = row.SingleOrDefault(p => p.Thickness == thickness);
								if (null == product)
									return 0;

								return ProductService.GetProductPrice(product.ProductId, filter.Date);
							})
						.ToArray();

					productPrices.Add(rowPrices);
				}

				return new PricelistModel
				{
					Columns = colNames.Select(p => p.ToString()).ToArray(),
					Rows = productNames.ToArray(),
					Prices = productPrices.ToArray(),
				};
			}
		}

		[HttpPost]
		[Route("api/price/get-competitor-prices")]
		public IEnumerable<CompetitorPriceModel> GetCompetitorPrices([FromBody] CompetitorPriceFilterModel filter)
		{
			Argument.NotNull(filter, "Filter is required.");

			filter.Date = filter.Date.Date;

			using (var storage = new Storage())
			{
				var query = storage
					.Products
					.LoadWith(p => p.Manufacturer)
					.Where(
						p => p.ManufacturerId != filter.ManufacturerId
							&& p.Thickness == filter.Thickness
							&& p.Name == filter.ProductName);

				if (filter.RollType != RollType.Undefined)
				{
					query = query
						.Where(
							p => p.RawMaterial.RawMaterialType.RollType == filter.RollType
								|| p.RawMaterial.RawMaterialType.RollType == RollType.Undefined);
				}

				if (filter.AlloyType != AlloyType.Undefined)
				{
					query = query
						.Where(
							p => p.RawMaterial.RawMaterialType.AlloyType == filter.AlloyType
								|| p.RawMaterial.RawMaterialType.AlloyType == AlloyType.Undefined);
				}

				var products = query
					.ToList()
					.GroupBy(p => p.ManufacturerId);

				foreach (var group in products)
				{
					var allPrices = group
						.Select(p => ProductService.GetProductPrice(p.ProductId, filter.Date))
						.ToArray();

					yield return new CompetitorPriceModel
					{
						ManufacturerId = group.Key,
						ManufacturerName = group.First().Manufacturer.Name,
						MultipleProducts = allPrices.Length > 1,
						MinPrice = allPrices.Min(),
						MaxPrice = allPrices.Max(),
					};
				}
			}
		}

		private void StoreProductPrices(BulkPriceAddModel model)
		{
			var priceExtraResult = ParserService.ParseExtraPricesBulk(
						model.ManufacturerId,
						model.SupplierId,
						model.Date,
						model.AlloyType,
						model.RollType,
						model.PriceExtraCategoryId,
						model.Prices);

			if (priceExtraResult.Errors.Any())
			{
				throw HttpException(HttpStatusCode.BadRequest, priceExtraResult.Errors.Join("<br/>"));
			}

			foreach (var product in priceExtraResult.Products)
			{
				var productId = product.ProductId;
				if (productId == 0)
				{
					productId = ProductService.CreateProduct(
						product.ManufacturerId,
						product.RawMaterialId,
						product.Name,
						product.Thickness).ProductId;
				}

				var extra = product.PriceExtras.Single();
				var priceItem = extra.PriceItems.Single();

				var priceExtra = ProductService.AddPriceExtra(productId, extra.PriceExtraCategoryId);

				ProductService.SetExtraPrice(priceExtra.PriceExtraId, model.ManufacturerId, model.Date, priceItem.Price);
			}
		}


		private void StoreMaterialPrices(BulkPriceAddModel model)
		{
			var materialsResult = ParserService.ParseMaterialPricesBulk(
						model.ManufacturerId,
						model.SupplierId,
						model.Date,
						model.AlloyType,
						model.RollType,
						model.Prices);

			if (materialsResult.Errors.Any())
			{
				throw HttpException(HttpStatusCode.BadRequest, materialsResult.Errors.Join("<br/>"));
			}

			foreach (var material in materialsResult.Materials)
			{
				var materialId = material.RawMaterialId;

				var priceItem = material.PriceItems.Single();

				ProductService.SetMaterialPrice(materialId, model.ManufacturerId, model.Date, priceItem.Price);
			}
		}
	}
}