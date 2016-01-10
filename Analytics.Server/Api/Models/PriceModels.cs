using System;
using Analytics.Core.Data.Enums;
using Newtonsoft.Json;

namespace Analytics.Server.Api.Models
{
	public enum PriceType
	{
		RawMaterial,
		PriceExtra,
	}

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
		public decimal[][] Prices { get; set; }
	}

	public class CompetitorPriceFilterModel
	{
		public string ProductName { get; set; }
		public RollType RollType { get; set; }
		public AlloyType AlloyType { get; set; }
		public decimal Thickness { get; set; }
		public int ManufacturerId { get; set; }
		public DateTime Date { get; set; }
	}

	public class CompetitorPriceModel
	{
		public int ManufacturerId { get; set; }
		public string ManufacturerName { get; set; }
		public bool MultipleProducts { get; set; }
		public decimal MinPrice { get; set; }
		public decimal MaxPrice { get; set; }
	}

	public class PriceFilterModel
	{
		public int ManufacturerId { get; set; }

		public RollType RollType { get; set; }

		public AlloyType AlloyType { get; set; }

		public DateTime? Date { get; set; }

	}
}