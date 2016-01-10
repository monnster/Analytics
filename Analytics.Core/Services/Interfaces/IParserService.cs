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
			string raw);

		BulkProductParseResult ParseExtraPricesBulk(
			int manufacturerId,
			int supplierId,
			DateTime date,
			AlloyType alloyType,
			RollType rollType,
			int priceExtraCategoryId,
			string raw
			);
	}
}