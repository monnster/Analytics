using System;

namespace Analytics.Core.Data.Entities
{
	public class PriceHistory
	{
		public DateTime Date { get; set; }

		public decimal? Price { get; set; }
	}
}