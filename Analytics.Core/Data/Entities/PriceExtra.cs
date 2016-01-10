using System;
using System.Collections;
using System.Collections.Generic;
using LinqToDB.Mapping;

namespace Analytics.Core.Data.Entities
{
	[Table("PriceExtra")]
	public class PriceExtra
	{
		[Column("PriceExtraId"), PrimaryKey, Identity, NotNull]
		public int PriceExtraId { get; set; }

		[Column("PriceExtraCategoryId"), NotNull]
		public int PriceExtraCategoryId { get; set; }

		[Column("ProductId"), NotNull]
		public int ProductId { get; set; }

		[Association(ThisKey = "PriceExtraId", OtherKey = "PriceExtraId", CanBeNull = false)]
		public IEnumerable<PriceItem> PriceItems { get; set; }

		[Association(ThisKey = nameof(PriceExtraCategoryId), OtherKey = nameof(PriceExtraCategoryId), CanBeNull = false)]
		public PriceExtraCategory PriceExtraCategory { get; set; }
	}
}