using System;
using System.Collections;
using System.Collections.Generic;
using Analytics.Core.Data.Enums;
using LinqToDB.Mapping;

namespace Analytics.Core.Data.Entities
{
	[Table("RawMaterialTypes")]
	public class RawMaterialType
	{
		[Column("RawMaterialTypeId"), PrimaryKey, Identity, NotNull]
		public int RawMaterialTypeId { get; set; }

		[Column("Name"), NotNull]
		public string Name { get; set; }

		[Column("AlloyType"), NotNull]
		public AlloyType AlloyType { get; set; }

		[Column("RollType"), NotNull]
		public RollType RollType { get; set; }

		[Column("Thickness"), NotNull]
		public decimal Thickness { get; set; }

		[Association(ThisKey = "RawMaterialTypeId", OtherKey = "RawMaterialTypeId", CanBeNull = true)]
		public IEnumerable<RawMaterial> RawMaterials { get; set; }
	}
}