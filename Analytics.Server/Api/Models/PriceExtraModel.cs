using System;

namespace Analytics.Server.Api.Models
{
	public class PriceExtraModel
	{
		public int ProductId { get; set; }

		public int PriceExtraId { get; set; }

		public int PriceCategoryId { get; set; }

		public string PriceCategoryName { get; set; }

		public decimal? LastPrice { get; set; }

		public DateTime LastDate { get; set; }
	}
}