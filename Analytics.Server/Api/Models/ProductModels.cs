using System;
using Analytics.Core.Data.Enums;

namespace Analytics.Server.Api.Models
{
	public class ProductModel
	{
		public int ProductId { get; set; }
		public int ManufacturerId { get; set; }
		public string ManufacturerName { get; set; }
		public int RawMaterialTypeId { get; set; }
		public string RawMaterialTypeName { get; set; }
		public decimal Thickness { get; set; }
	}

	public class ProductFilter
	{
		public AlloyType? AlloyType { get; set; }

		public RollType? RollType { get; set; }

		public int? ManufacturerId { get; set; }

		public DateTime? Date { get; set; }

		public string Name { get; set; }

		public decimal? Thickness { get; set; }
	}

	public class PriceHistoryFilter
	{
		public int ProductId { get; set; }

		public DateTime DateFrom { get; set; }

		public DateTime DateTo { get; set; }
	}
}