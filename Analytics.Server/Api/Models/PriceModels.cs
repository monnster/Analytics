using System;
using Analytics.Core.Data.Entities;
using Analytics.Core.Data.Enums;
using Newtonsoft.Json;

namespace Analytics.Server.Api.Models
{
	public class BulkPriceAddModel
	{
		public int ManufacturerId { get; set; }
		public int SupplierId { get; set; }
		public RollType RollType { get; set; }
		public AlloyType AlloyType { get; set; }
		public PriceType PriceType { get; set; }
		public int PriceExtraCategoryId { get; set; }
		public DateTime Date { get; set; }
		public string Prices { get; set; }
	}

	public class PricelistModel
	{
		public string[] Columns { get; set; }
		public string[] Rows { get; set; }
		public PriceModel[][] Prices { get; set; }
	}

	public class PriceModel
	{
		public string Name { get; set; }
		public decimal Thickness { get; set; }
		public decimal? Price { get; set; }
		public decimal? RetailPrice { get; set; }
	}

	public class CompetitorPriceFilterModel
	{
		public string ProductName { get; set; }
		public RollType RollType { get; set; }
		public AlloyType AlloyType { get; set; }
		public int ManufacturerId { get; set; }
		public DateTime Date { get; set; }
		public decimal[] Thicknesses { get; set; }
	}

	public class CompetitorPriceModel
	{
		public int ManufacturerId { get; set; }
		public string ManufacturerName { get; set; }
		public int SupplierId { get; set; }
		public decimal?[] Prices { get; set; }

	}

	public class PriceFilterModel
	{
		public int ManufacturerId { get; set; }

		public RollType RollType { get; set; }

		public AlloyType AlloyType { get; set; }

		public DateTime? Date { get; set; }

	}
}