using Analytics.Core.Data.Enums;

namespace Analytics.Core.Data.Entities
{
	public class ProductExt
	{
		public int ProductId { get; set; }
		public int ManufacturerId { get; set; }
		public string ManufacturerName { get; set; }
		public int RawMaterialTypeId { get; set; }
		public string RawMaterialTypeName { get; set; }
		public AlloyType AlloyType { get; set; }
		public RollType RollType { get; set; }
		public decimal Thickness { get; set; }
		public decimal? Price { get; set; }
		public string Name { get; set; }
	}
}