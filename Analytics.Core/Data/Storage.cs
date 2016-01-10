using Analytics.Core.Data.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace Analytics.Core.Data
{
	public class Storage: DataConnection
	{
		public ITable<Manufacturer> Manufacturers { get { return GetTable<Manufacturer>(); } } 
		public ITable<RawMaterial> RawMaterials { get { return GetTable<RawMaterial>(); } } 
		public ITable<RawMaterialType> RawMaterialTypes { get { return GetTable<RawMaterialType>(); } } 
		public ITable<PriceExtraCategory> PriceExtraCategories { get { return GetTable<PriceExtraCategory>(); } } 
		public ITable<PriceExtra> PriceExtras { get { return GetTable<PriceExtra>(); } } 
		public ITable<PriceCache> PriceCache { get { return GetTable<PriceCache>(); } } 
		public ITable<PriceItem> PriceItems { get { return GetTable<PriceItem>(); } } 
		public ITable<Product> Products { get { return GetTable<Product>(); } }


		public Storage()
		{
		}

		public Storage(string configurationString) 
			: base(configurationString)
		{
		}
	}
}