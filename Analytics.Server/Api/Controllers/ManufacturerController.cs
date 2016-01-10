using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Analytics.Core;
using Analytics.Core.Data;
using Analytics.Core.Data.Entities;
using LinqToDB;

namespace Analytics.Server.Api.Controllers
{
	//[Route("api/dict/manufacturer")]
	public class ManufacturerController: ApiControllerBase
	{
		public Manufacturer Get(int id)
		{
			using (var storage = new Storage())
			{
				return storage
					.Manufacturers
					.SingleOrDefault(m => m.ManufacturerId == id);
			}
		}

		public IEnumerable<Manufacturer> Get()
		{
			using (var storage = new Storage())
			{
				return storage
					.Manufacturers
					.ToList();
			}
		}

		public void Post([FromBody] Manufacturer manufacturer)
		{
			Argument.NotNull(manufacturer, "Manufacturer is required.");

			if (manufacturer.IsPrimary)
				return;

			using (var storage = new Storage())
			{
				var existingManufacturer = storage
					.Manufacturers
					.SingleOrDefault(m => m.ManufacturerId == manufacturer.ManufacturerId);

				if (null != existingManufacturer)
				{
					storage.Update(manufacturer);
				}
				else
				{
					storage.Insert(manufacturer);
				}

			}
		}

		public void Delete(int id)
		{
			throw new NotSupportedException("Manufacturers can not be deleted.");
		}
	}
}