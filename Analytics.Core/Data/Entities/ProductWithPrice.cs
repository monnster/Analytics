namespace Analytics.Core.Data.Entities
{
	public class ProductWithPrice: Product
	{
		public decimal? Price { get; set; }
		public decimal? RetailPrice { get; set; }
	}
}