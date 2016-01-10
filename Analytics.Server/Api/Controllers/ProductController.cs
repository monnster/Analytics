using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Analytics.Core;
using Analytics.Core.Data;
using Analytics.Core.Data.Entities;
using Analytics.Core.Services.Interfaces;
using Analytics.Server.Api.Models;
using LinqToDB;

namespace Analytics.Server.Api.Controllers
{
	//[Route("api/product")]
	public class ProductController: ApiControllerBase
	{
		public IProductService ProductService { get; set; }

		[HttpGet]
		[Route("api/product/getPriceExtras")]
		public IEnumerable<PriceExtraModel> GetPriceExtras(int id)
		{
			var date = DateTime.Today;

			using (var storage = new Storage())
			{
				var product = storage
					.Products
					.LoadWith(p => p.PriceExtras)
					.LoadWith(p => p.PriceExtras[0].PriceExtraCategory)
					.SingleOrDefault(p => p.ProductId == id);

				if (null == product)
					yield break;

				foreach (var priceExtra in product.PriceExtras)
				{
					var lastPrice = storage
						.PriceItems
						.Where(pi => pi.PriceExtraId == priceExtra.PriceExtraId && pi.Date <= date)
						.OrderByDescending(pi => pi.Date)
						.FirstOrDefault();

					yield return new PriceExtraModel
					{
						ProductId = id,
						PriceExtraId = priceExtra.PriceExtraId,
						PriceCategoryId = priceExtra.PriceExtraCategoryId,
						PriceCategoryName = priceExtra.PriceExtraCategory.Name,
						LastDate = lastPrice?.Date ?? date,
						LastPrice = lastPrice?.Price
					};
				}

			}
		}

		[HttpGet]
		[HttpPost]
		[Route("api/product/getPriceHistory")]
		public IEnumerable<PriceHistory> GetPriceHistory([FromBody]PriceHistoryFilter filter)
		{
			Argument.NotNull(filter, "Filter is required.");

			return ProductService.GetPriceHistory(filter.ProductId, filter.DateFrom, filter.DateTo);
		}

		public ProductModel Get(int id)
		{
			using (var storage = new Storage())
			{
				return storage
					.Products
					.LoadWith(p => p.Manufacturer)
					.LoadWith(p => p.RawMaterial.RawMaterialType)
					.Where(p => p.ProductId == id)
					.Select(p => new ProductModel
					{
						ProductId = p.ProductId,
						ManufacturerId = p.ManufacturerId,
						ManufacturerName = p.Manufacturer.Name,
						RawMaterialTypeId = p.RawMaterial.RawMaterialTypeId,
						RawMaterialTypeName = p.RawMaterial.RawMaterialType.Name,
						Thickness = p.Thickness,
					})
					.SingleOrDefault();
			}
		}

		public IEnumerable<ProductModel> Get()
		{
			using (var storage = new Storage())
			{
				return storage
					.Products
					.LoadWith(p => p.Manufacturer)
					.LoadWith(p => p.RawMaterial.RawMaterialType)
					.Select(p => new ProductModel
					{
						ProductId = p.ProductId,
						ManufacturerId = p.ManufacturerId,
						ManufacturerName = p.Manufacturer.Name,
						RawMaterialTypeId = p.RawMaterial.RawMaterialTypeId,
						RawMaterialTypeName = p.RawMaterial.RawMaterialType.Name,
						Thickness = p.Thickness,
					})
					.ToList();
			}
		}

		[HttpPost]
		[Route("api/product/getFiltered")]
		public IEnumerable<ProductExt> GetFiltered([FromBody] ProductFilter filter)
		{
			return ProductService.FindProducts(
					filter.ManufacturerId,
					filter.Name,
					filter.Thickness,
					filter.AlloyType,
					filter.RollType);
		}

		[HttpPost]
		[Route("api/product/getFilteredWithPrices")]
		public IEnumerable<ProductExt> GetFilteredWithPrices([FromBody] ProductFilter filter)
		{
			return ProductService.FindProducts(
					filter.ManufacturerId,
					filter.Name,
					filter.Thickness,
					filter.AlloyType,
					filter.RollType)
				.Select(
					p =>
					{
						p.Price = ProductService
							.GetProductWithPrice(p.ProductId, filter.Date)
							.Price;

						return p;
					});
		} 

		public void Post([FromBody] Product product)
		{
			Argument.NotNull(product, "Product is required.");

			using (var storage = new Storage())
			{
				var existingProduct = storage
					.Products
					.SingleOrDefault(p => p.ProductId == product.ProductId);

				if (null != existingProduct)
				{
					storage.Update(product);
				}
				else
				{
					storage.Insert(product);
				}
			}
		}

		
		public IHttpActionResult Delete(int id)
		{
			Argument.Require(id > 0, "Product id is required.");

			using (var storage = new Storage())
			{
				storage
					.Products
					.Delete(p => p.ProductId == id);
			}

			return Ok();
		}
	}
}