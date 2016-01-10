using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using Analytics.Core.Data;
using Analytics.Core.Data.Entities;
using Analytics.Core.Data.Enums;
using Analytics.Core.Services.Interfaces;
using EmitMapper;
using LinqToDB;

namespace Analytics.Core.Services.Impl
{
	public class ProductService: IProductService
	{
		private static ObjectsMapper<Product, ProductWithPrice> _productMapper =
			ObjectMapperManager.DefaultInstance.GetMapper<Product, ProductWithPrice>();

		public Product CreateProduct(int manufacturerId, int rawMaterialId, string size, decimal thickness)
		{
			Argument.Require(manufacturerId > 0, "Manufacturer id is required.");
			Argument.Require(rawMaterialId > 0, "Raw material id is required.");
			Argument.NotNullOrEmpty(size, "Product size is required.");
			Argument.Require(thickness > 0, "Product thickness is required.");

			var product = new Product
			{
				ManufacturerId = manufacturerId,
				RawMaterialId = rawMaterialId,
				Name = size,
				Thickness = thickness,
			};

			using (var storage = new Storage())
			{
				product.ProductId = (int)(long)storage.InsertWithIdentity(product);
			}

			return product;
		}

		public PriceExtra AddPriceExtra(int productId, int priceExtraCategoryId)
		{
			Argument.Require(productId > 0, "Product identifier is required.");
			Argument.Require(priceExtraCategoryId > 0, "Price extra category id is required.");

			var priceExtra = new PriceExtra
			{
				PriceExtraCategoryId = priceExtraCategoryId,
				ProductId = productId,
			};

			using (var storage = new Storage())
			{
				var existingExtra = storage
					.PriceExtras
					.SingleOrDefault(pe => pe.ProductId == productId && pe.PriceExtraCategoryId == priceExtraCategoryId);

				if (null == existingExtra)
				{
					priceExtra.PriceExtraId = (int) (long) storage.InsertWithIdentity(priceExtra);
				}
				else
				{
					priceExtra.PriceExtraId = existingExtra.PriceExtraId;
				}
			}

			return priceExtra;
		}

		public void RemovePriceExtra(int productId, int priceExtraId, DateTime? date = null)
		{
			Argument.Require(productId > 0, "Product id is required.");
			Argument.Require(priceExtraId > 0, "Price extra id is required.");

			using (var storage = new Storage())
			{
				var existingPriceExtra = storage
					.PriceExtras
					.SingleOrDefault(pe => pe.PriceExtraId == priceExtraId);

				if(null == existingPriceExtra)
					return;

				storage.Delete(existingPriceExtra);
				
				UpdatePriceCache(storage, existingPriceExtra.ProductId, date ?? DateTime.Today);
			}
		}

		public void SetExtraPrice(int priceExtraId, int ownerId, DateTime date, decimal price)
		{
			Argument.Require(priceExtraId > 0, "Price extra id is required.");
			Argument.Require(ownerId > 0, "Owner id is required.");
			Argument.Require(date > DateTime.MinValue, "Price date is required.");
			Argument.Require(price >= 0, "Price should be >= 0");

			using (var storage = new Storage())
			{
				var priceExtra = storage
					.PriceExtras
					.SingleOrDefault(pe => pe.PriceExtraId == priceExtraId);

				if(null == priceExtra)
					throw new InvalidOperationException($"Price extra with id {priceExtraId} not found.");

				var existingPriceItem = storage
					.PriceItems
					.SingleOrDefault(pi => pi.PriceExtraId == priceExtraId && pi.OwnerId == ownerId && pi.Date == date);

				if (null != existingPriceItem)
				{
					existingPriceItem.Price = price;
					storage.Update(existingPriceItem);
				}
				else
				{
					var priceItem = new PriceItem
					{
						PriceExtraId = priceExtraId,
						Date = date,
						OwnerId = ownerId,
						Price = price,
					};

					storage.Insert(priceItem);
				}

				UpdatePriceCache(storage, priceExtra.ProductId, date);
			}
		}

		public void SetMaterialPrice(int rawMaterialId, int ownerId, DateTime date, decimal price)
		{
			Argument.Require(rawMaterialId > 0, "Raw material identifier is required.");
			Argument.Require(ownerId > 0, "Owner identifier is required.");
			Argument.Require(date > DateTime.MinValue, "Date is required.");
			Argument.Require(price >= 0, "Price should be >= 0.");

			using (var storage = new Storage())
			{
				var priceExtra = storage
					.RawMaterials
					.SingleOrDefault(pe => pe.RawMaterialId == rawMaterialId);

				if (null == priceExtra)
					throw new InvalidOperationException($"Raw material with id {rawMaterialId} not found.");

				var existingPriceItem = storage
					.PriceItems
					.SingleOrDefault(pi => pi.RawMaterialId == rawMaterialId && pi.OwnerId == ownerId && pi.Date == date);

				if (null != existingPriceItem)
				{
					existingPriceItem.Price = price;
					storage.Update(existingPriceItem);
				}
				else
				{
					var priceItem = new PriceItem
					{
						RawMaterialId = rawMaterialId,
						Date = date,
						OwnerId = ownerId,
						Price = price,
					};

					storage.Insert(priceItem);
				}

				var products = storage
					.Products
					.Where(product => product.ManufacturerId == ownerId && product.RawMaterialId == rawMaterialId)
					.Select(p => p.ProductId)
					.ToList();

				foreach (var productId in products)
				{
					UpdatePriceCache(storage, productId, date);
				}

			}
		}

		public ProductWithPrice GetProductWithPrice(int productId, DateTime? date = null)
		{
			Argument.Require(productId > 0, "Product identifier is required.");

			date = date ?? DateTime.Today;

			using (var storage = new Storage())
			{
				var product = storage
					.Products
					.SingleOrDefault(p => p.ProductId == productId);

				if (null == product)
					return null;

				var productWithPrice = _productMapper.Map(product);

				productWithPrice.Price = GetProductPriceInternal(storage, productId, date.Value);

				return productWithPrice;
			}
		}

		public decimal GetProductPrice(int productId, DateTime? date = null)
		{
			Argument.Require(productId > 0, "Product id is required");

			using (var storage = new Storage())
			{
				return GetProductPriceInternal(storage, productId, date ?? DateTime.Today);
			}
		}

		public IEnumerable<ProductExt> FindProducts(int? manufacturerId, string name, decimal? thickness, AlloyType? alloyType, RollType? rollType)
		{
			using (var storage = new Storage())
			{
				var query = storage
					.Products
					.LoadWith(p => p.Manufacturer)
					.LoadWith(p => p.RawMaterial.RawMaterialType)
					.Where(product => true);

				if (null != manufacturerId)
				{
					query = query.Where(product => product.ManufacturerId == manufacturerId.Value);
				}

				if (!string.IsNullOrEmpty(name))
				{
					query = query.Where(product => product.Name == name);
				}

				if (null != thickness)
				{
					query = query.Where(product => product.Thickness == thickness.Value);
				}

				if (null != alloyType)
				{
					query = query.Where(product => product.RawMaterial.RawMaterialType.AlloyType == alloyType.Value);
				}

				if (null != rollType)
				{
					query = query.Where(product => product.RawMaterial.RawMaterialType.RollType == rollType.Value);
				}

				return query
					.ToList()
					.Select(p => new ProductExt
					{
						ProductId = p.ProductId,
						ManufacturerId = p.ManufacturerId,
						ManufacturerName = p.Manufacturer.Name,
						RawMaterialTypeId = p.RawMaterial.RawMaterialType.RawMaterialTypeId,
						RawMaterialTypeName = p.RawMaterial.RawMaterialType.Name,
						AlloyType = p.RawMaterial.RawMaterialType.AlloyType,
						RollType = p.RawMaterial.RawMaterialType.RollType,
						Thickness = p.Thickness,
						Name = p.Name,
					});
			}
		}

		public IEnumerable<PriceHistory> GetPriceHistory(int productId, DateTime dateFrom, DateTime dateTo)
		{
			Argument.Require(productId > 0, "Product id is required.");
			Argument.Require(dateFrom < dateTo, "Invalid date interval.");

			using (var storage = new Storage())
			{
				var product = storage
					.Products
					.SingleOrDefault(p => p.ProductId == productId);

				if (null == product)
					return Enumerable.Empty<PriceHistory>();

				return storage
					.PriceCache
					.Where(pc => pc.ProductId == productId)
					.Where(pc => dateFrom <= pc.Date && pc.Date <= dateTo)
					.OrderBy(pc => pc.Date)
					.ToList()
					.Select(pc => new PriceHistory
					{
						Date = pc.Date,
						Price = pc.Price,
					});
			}

		}

		private decimal GetProductPriceInternal(Storage storage, int productId, DateTime date)
		{
			Argument.NotNull(storage, "Storage is required.");

			var priceCache = storage
					.PriceCache
					.Where(p => p.ProductId == productId && p.Date <= date)
					.OrderByDescending(p => p.Date)
					.FirstOrDefault();

			return priceCache?.Price ?? 0;
		}

		private void UpdatePriceCache(Storage storage, int productId, DateTime date)
		{
			Argument.NotNull(storage, "Storage is required.");
			Argument.Require(productId > 0, "Product id is required");
			Argument.Require(date > DateTime.MinValue, "Date is required.");

			UpdatePriceCacheForDate(storage, productId, date);

			var futurePriceDates = storage
				.PriceCache
				.Where(pc => pc.Date > date && pc.ProductId == productId)
				.Select(pc => pc.Date)
				.ToList();

			foreach (var futurePrice in futurePriceDates)
			{
				UpdatePriceCacheForDate(storage, productId,	futurePrice);
			}
		}

		private void UpdatePriceCacheForDate(Storage storage, int productId, DateTime date)
		{
			var product = storage
				.Products
				.SingleOrDefault(p => p.ProductId == productId);

			if (null == product)
				throw new InvalidOperationException($"Product with id {productId} not found.");

			var totalPrice = storage
				.PriceItems
				.Where(pi => pi.PriceExtra.ProductId == productId && pi.Date <= date)
				.ToList()
				.GroupBy(p => p.PriceExtraId)
				.SelectMany(group => group
					.OrderByDescending(p => p.Date)
					.Take(1)
					.Select(p => p.Price)
				)
				.Sum();

			if (product.RawMaterialId > 0)
			{
				var rawMaterialPrice = storage
					.PriceItems
					.Where(pi => pi.RawMaterialId == product.RawMaterialId && pi.Date <= date)
					.OrderByDescending(pi => pi.Date)
					.FirstOrDefault();

				if (null != rawMaterialPrice)
				{
					totalPrice += rawMaterialPrice.Price;
				}
			}

			var priceCache = new PriceCache
			{
				ProductId = productId,
				Date = date,
				Price = totalPrice,
			};

			storage.InsertOrReplace(priceCache);
		}
	}
}