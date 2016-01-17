using System;
using LinqToDB.Mapping;

namespace Analytics.Core.Data.Entities
{
	[Table("PriceExtraCategory")]
	public class PriceExtraCategory
	{
		[Column("PriceExtraCategoryId"), PrimaryKey, Identity, NotNull]
		public int PriceExtraCategoryId { get; set; }

		[Column("Name"), NotNull]
		public string Name { get; set; }
	}
}