using System;
using System.Diagnostics;
using System.Linq;
using Analytics.Core.Data;
using Analytics.Core.Data.Entities;
using LinqToDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analytics.Tests.Data
{
	[TestClass]
	public class StorageTests: RepositoryTestBase
	{
		[TestMethod]
		public void StorageShouldCreateManufacturers()
		{
			using (var storage = new Storage())
			{
				var result = storage.InsertWithIdentity(new Manufacturer
				{
					IsPrimary = true,
					Name = "Severstahl",
				});

				var result2 = storage.InsertWithIdentity(new Manufacturer
				{
					IsPrimary = true,
					Name = "Severstahl2",
				});

				Debug.WriteLine(result);
				Debug.WriteLine(result2);

				Assert.AreEqual(2, storage.Manufacturers.ToList().Count);
			}
		}

		
	}
}