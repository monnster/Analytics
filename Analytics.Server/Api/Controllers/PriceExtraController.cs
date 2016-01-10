using System.Linq;
using System.Web.Http;
using Analytics.Core;
using Analytics.Core.Data;
using Analytics.Core.Data.Entities;
using Analytics.Core.Services.Interfaces;
using LinqToDB;

namespace Analytics.Server.Api.Controllers
{
	//[Route("api/price-extra")]
	public class PriceExtraController: ApiControllerBase
	{
		public IProductService ProductService { get; set; }

		public void Post([FromBody] PriceExtra extra)
		{
			Argument.NotNull(extra, "Price extra is required.");

			ProductService.AddPriceExtra(extra.ProductId, extra.PriceExtraCategoryId);
		}

		public PriceExtra Get(int id)
		{
			using (var storage = new Storage())
			{
				return storage
					.PriceExtras
					.SingleOrDefault(pe => pe.PriceExtraId == id);
			}
		}

		public void Delete(int priceExtraId)
		{
			Argument.NotNull(priceExtraId > 0, "Price extra id is required.");

			ProductService.RemovePriceExtra(1, priceExtraId);
		}

	}
}