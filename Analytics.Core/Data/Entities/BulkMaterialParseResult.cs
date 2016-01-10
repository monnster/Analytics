namespace Analytics.Core.Data.Entities
{
	public class BulkMaterialParseResult
	{
		public RawMaterial[] Materials { get; set; }

		public string[] Errors { get; set; }
	}
}