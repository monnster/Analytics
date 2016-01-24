using System;
using System.Data.SQLite;
using System.Security.Cryptography;
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

		[Column("Price", Scale = 10, Precision = 2), Nullable]
		public decimal? Price { get; set; }

		[Column("RetailPrice", Scale = 10, Precision = 2), Nullable]
		public decimal? RetailPrice { get; set; }

		[Association(ThisKey = nameof(ProductId), OtherKey = nameof(ProductId), CanBeNull = false)]
		public Product Product { get; set; }

		[Association(ThisKey = nameof(OwnerId), OtherKey = nameof(Manufacturer.ManufacturerId))]
		public Manufacturer Owner { get; set; }

	}
}