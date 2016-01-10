using System;
using Analytics.Core.Data;
using Analytics.Core.Data.Entities;
using Analytics.Core.Data.Enums;
using Analytics.Core.Services.Impl;
using LinqToDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analytics.Tests.Data
{
	[TestClass]
	public class ProductServiceTests: RepositoryTestBase
	{
		[TestMethod]
		public void ServiceShouldCreateProducts()
		{
			using (var storage = new Storage())
			{
				// RawMaterialTypeId = 1
				storage.Insert(
					new RawMaterialType
					{
						RawMaterialTypeId = 1,
						AlloyType = AlloyType.Regular,
						RollType = RollType.Undefined,
						Name = "Штрипс",
						Thickness = 10,
					});

				

				// PriceExtraCategoryId = 1
				storage.Insert(
					new PriceExtraCategory
					{
						Name = "Доставка"
					});

				// PriceExtraCategoryId = 2
				storage.Insert(
					new PriceExtraCategory
					{
						Name = "Передел"
					});

				// ManufacturerId = 1
				storage.Insert(
					new Manufacturer
					{
						Name = "Главный Производитель",
						IsPrimary = true,
					});

				// ManufacturerId = 1
				storage.Insert(
					new Manufacturer
					{
						Name = "Субпроизводитель",
						IsPrimary = false,
					});

				// RawMaterialId = 1,
				storage.Insert(
					new RawMaterial
					{
						ManufacturerId = 1,
						RawMaterialTypeId = 1,
					});
			}

			var service = new ProductService();
			var product = service.CreateProduct(2, 1, "10x10", 2);

			var extra1 = service.AddPriceExtra(product.ProductId, 1);
			var extra2 = service.AddPriceExtra(product.ProductId, 2);

			service.SetMaterialPrice(1, 2, new DateTime(2015, 1, 1), 100);
			service.SetExtraPrice(extra1.PriceExtraId, 2, new DateTime(2015, 1, 1), 200);
			service.SetExtraPrice(extra2.PriceExtraId, 2, new DateTime(2015, 1, 2), 300);

			var price1 = service.GetProductWithPrice(product.ProductId).Price;

			service.SetMaterialPrice(1, 2, new DateTime(2015, 4, 1), 150);
			service.SetExtraPrice(extra1.PriceExtraId, 2, new DateTime(2015, 3, 1), 400);

			var price2 = service.GetProductWithPrice(product.ProductId).Price;

			Assert.AreEqual(600, price1);
			Assert.AreEqual(850, price2);

		}
	}
}