namespace Analytics.Core.Data.Entities
{
	public class BulkProductParseResult
	{
		public Product[] Products { get; set; }

		public string[] Errors { get; set; }
	}
}