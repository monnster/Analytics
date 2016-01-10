using System;
using System.Data.SQLite;
using LinqToDB.Mapping;

namespace Analytics.Core.Data.Entities
{
	[Table("PriceCache")]
	public class PriceCache
	{
		[Column("ProductId"), NotNull, PrimaryKey]
		public int ProductId { get; set; }

		[Column("Date"), NotNull, PrimaryKey]
		public DateTime Date { get; set; }

		[Column("OwnerId"), NotNull, PrimaryKey]
		public int OwnerId { get; set; }

		[Column("Price"), NotNull]
		public decimal Price { get; set; }

		[Association(ThisKey = nameof(ProductId), OtherKey = nameof(ProductId), CanBeNull = false)]
		public Product Product { get; set; }

		[Association(ThisKey = nameof(OwnerId), OtherKey = nameof(Manufacturer.ManufacturerId))]
		public Manufacturer Owner { get; set; }
	}
}