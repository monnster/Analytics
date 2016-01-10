using Analytics.Core.Data;
using Analytics.Core.Data.Entities;
using LinqToDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analytics.Tests.Data
{
	public abstract class RepositoryTestBase
	{
		[TestInitialize]
		public void SetupStorage()
		{
			try
			{
				ClearStorage();
			}
			catch
			{
			}

			using (var storage = new Storage())
			{
				storage.CreateTable<Manufacturer>();
				storage.CreateTable<RawMaterialType>();
				storage.CreateTable<RawMaterial>();
				storage.CreateTable<PriceExtraCategory>();
				storage.CreateTable<PriceExtra>();
				storage.CreateTable<PriceItem>();
				storage.CreateTable<Product>();
				storage.CreateTable<PriceCache>();
			}
		}

		[TestCleanup]
		public void ClearStorage()
		{
			using (var storage = new Storage())
			{
				storage.DropTable<Manufacturer>();
				storage.DropTable<RawMaterialType>();
				storage.DropTable<RawMaterial>();
				storage.DropTable<PriceExtraCategory>();
				storage.DropTable<PriceExtra>();
				storage.DropTable<PriceItem>();
				storage.DropTable<Product>();
				storage.DropTable<PriceCache>();
			}
		}
	}
}