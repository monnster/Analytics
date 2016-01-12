using System;
using System.Collections;
using System.Collections.Generic;
using Analytics.Core.Data.Entities;
using Analytics.Core.Data.Enums;

namespace Analytics.Core.Services.Interfaces
{
	public interface IProductService
	{
		Product CreateProduct(int manufacturerId, int rawMaterialId, string size, decimal thickness);

		PriceExtra AddPriceExtra(int productId, int priceExtraCategoryId);

		void RemovePriceExtra(int productId, int priceExtraId, DateTime? date = null);

		void SetExtraPrice(int priceExtraId, int ownerId, DateTime date, decimal price);

		void SetMaterialPrice(int rawMaterialId, int ownerId, DateTime date, decimal price);

		void SetRetailPrice(int productId, DateTime date, decimal price);

		ProductWithPrice GetProductWithPrice(int productId, DateTime? date = null);

		decimal? GetProductPrice(int productId, DateTime? date = null);

		decimal? GetProductRetailPrice(int productId, DateTime? date = null);

		IEnumerable<ProductExt> FindProducts(int? manufacturerId, string name, decimal? thickness, AlloyType? alloyType, RollType? rollType);

		IEnumerable<PriceHistory> GetPriceHistory(int productId, DateTime dateFrom, DateTime dateTo);
	}
}