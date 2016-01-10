using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Analytics.Core;
using Analytics.Core.Data;
using Analytics.Core.Data.Entities;
using LinqToDB;

namespace Analytics.Server.Api.Controllers
{
	//[Route("api/dict/price-extra-category")]
	public class PriceExtraCategoryController: ApiControllerBase
	{
		public PriceExtraCategory Get(int id)
		{
			using (var storage = new Storage())
			{
				return storage
					.PriceExtraCategories
					.SingleOrDefault(pc => pc.PriceExtraCategoryId == id);
			}
		}

		public IEnumerable<PriceExtraCategory> Get()
		{
			using (var storage = new Storage())
			{
				return storage.PriceExtraCategories.ToList();
			}
		}

		public PriceExtraCategory Post([FromBody] PriceExtraCategory priceExtraCategory)
		{
			Argument.NotNull(priceExtraCategory, "Price extra category is required.");

			using (var storage = new Storage())
			{
				var existingCategory = storage
					.PriceExtraCategories
					.SingleOrDefault(pc => pc.PriceExtraCategoryId == priceExtraCategory.PriceExtraCategoryId);

				if (null != existingCategory)
				{
					storage.Update(priceExtraCategory);
				}
				else
				{
					storage.Insert(priceExtraCategory);
				}

				return priceExtraCategory;
			}
		}

		public void Delete(int id)
		{
			using (var storage = new Storage())
			{
				storage
					.PriceExtraCategories
					.Delete(pc => pc.PriceExtraCategoryId == id);
			}
		}
	}
}