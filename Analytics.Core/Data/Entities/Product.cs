using System;
using System.Collections.Generic;
using LinqToDB.Mapping;

namespace Analytics.Core.Data.Entities
{
	[Table("Product")]
	public class Product
	{
		[Column("ProductId"), PrimaryKey, Identity, NotNull]
		public int ProductId { get; set; }

		[Column("ManufacturerId"), NotNull]
		public int ManufacturerId { get; set; }

		[Column("RawMaterialId"), NotNull]
		public int RawMaterialId { get; set; }

		[Column("Name"), NotNull]
		public string Name { get; set; }

		[Column("Thickness", Scale = 2, Precision = 10), NotNull]
		public decimal Thickness { get; set; }

		#region Associations

		[Association(ThisKey = "ProductId", OtherKey = "ProductId", CanBeNull = true, IsBackReference = true)]
		public PriceExtra[] PriceExtras { get; set; }

		[Association(ThisKey = "ManufacturerId", OtherKey = "ManufacturerId", CanBeNull = false)]
		public Manufacturer Manufacturer { get; set; } 

		[Association(ThisKey = "RawMaterialId", OtherKey = "RawMaterialId", CanBeNull = false)]
		public RawMaterial RawMaterial { get; set; }

		#endregion
	}
}