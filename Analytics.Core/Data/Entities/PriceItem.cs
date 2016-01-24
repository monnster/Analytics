using System;
using System.Data.Odbc;
using LinqToDB.Mapping;

namespace Analytics.Core.Data.Entities
{
	[Table("PriceItem")]
	public class PriceItem
	{
		[Column("PriceItemId"), Identity, PrimaryKey, NotNull]
		public int PriceItemId { get; set; }

		[Column("PriceType"), NotNull]
		public PriceType PriceType { get; set; }

		[Column("RawMaterialId"), Nullable]
		public int RawMaterialId { get; set; }

		[Column("PriceExtraId"), Nullable]
		public int PriceExtraId { get; set; }

		[Column("ProductId"), Nullable]
		public int ProductId { get; set; }

		[Column("OwnerId"), CanBeNull]
		public int OwnerId { get; set; }

		[Column("Date"), NotNull]
		public DateTime Date { get; set; }

		[Column("Price", Scale = 10, Precision = 2), NotNull]
		public decimal Price { get; set; }

		[Association(ThisKey = "RawMaterialId", OtherKey = "RawMaterialId", CanBeNull = true)]
		public RawMaterial RawMaterial { get; set; }

		[Association(ThisKey = "PriceExtraId", OtherKey = "PriceExtraId", CanBeNull = true)]
		public PriceExtra PriceExtra { get; set; }

		[Association(ThisKey = "OwnerId", OtherKey = "ManufacturerId", CanBeNull = true)]
		public Manufacturer Owner { get; set; }

		[Association(ThisKey = nameof(ProductId), OtherKey = nameof(ProductId), CanBeNull = true)]
		public Product Product { get; set; }
	}
}