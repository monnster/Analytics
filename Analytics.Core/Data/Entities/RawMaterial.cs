using LinqToDB.Mapping;
using System.Collections.Generic;

namespace Analytics.Core.Data.Entities
{
	[Table("RawMaterial")]
	public class RawMaterial
	{
		[Column("RawMaterialId"), PrimaryKey, Identity, NotNull]
		public int RawMaterialId { get; set; }

		[Column("RawMaterialTypeId"), NotNull]
		public int RawMaterialTypeId { get; set; }

		[Column("ManufacturerId"), NotNull]
		public int ManufacturerId { get; set; }

		[Association(ThisKey = "RawMaterialTypeId", OtherKey = "RawMaterialTypeId", CanBeNull = false)]
		public RawMaterialType RawMaterialType { get; set; }

		[Association(ThisKey = "ManufacturerId", OtherKey = "ManufacturerId", CanBeNull = false)]
		public Manufacturer Manufacturer { get; set; }

		[Association(ThisKey = "RawMaterialId", OtherKey = "RawMaterialId", CanBeNull = true)]
		public IEnumerable<Product> Products { get; set; }

		[Association(ThisKey = "RawMaterialId", OtherKey = "RawMaterialId", CanBeNull = true)]
		public IEnumerable<PriceItem> PriceItems { get; set; }
	}
}