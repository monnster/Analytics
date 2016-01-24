using System;
using Analytics.Core.Data.Entities;
using Analytics.Core.Data.Enums;

namespace Analytics.Core.Services.Interfaces
{
	public interface IParserService
	{
		BulkMaterialParseResult ParseMaterialPricesBulk(
			int manufacturerId,
			int supplierId,
			DateTime date,
			AlloyType alloyType,
			RollType rollType,
			bool remove,
			string raw);

		BulkProductParseResult ParseExtraPricesBulk(
			int manufacturerId,
			int supplierId,
			DateTime date,
			AlloyType alloyType,
			RollType rollType,
			int priceExtraCategoryId,
			bool remove,
			string raw
			);
	}
}