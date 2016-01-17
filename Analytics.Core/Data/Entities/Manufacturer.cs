using System;
using System.Collections;
using System.Collections.Generic;
using LinqToDB.Mapping;

namespace Analytics.Core.Data.Entities
{
	[Table("Manufacturer")]
	public class Manufacturer
	{
		[Column("ManufacturerId"), Identity, PrimaryKey, NotNull]
		public int ManufacturerId { get; set; } 

		[Column("Name"), NotNull]
		public string Name { get; set; }

		[Column("IsPrimary"), NotNull]
		public bool IsPrimary { get; set; }

		[Association(ThisKey = "ManufacturerId", OtherKey = "ManufacturerId", CanBeNull = true)]
		public IEnumerable<RawMaterial> RawMaterialis { get; set; }

		[Association(ThisKey = "ManufacturerId", OtherKey = "ManufacturerId", CanBeNull = true)]
		public IEnumerable<Product> Products { get; set; }
	}
}