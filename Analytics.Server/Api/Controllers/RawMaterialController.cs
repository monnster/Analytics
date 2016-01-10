using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Analytics.Core;
using Analytics.Core.Data;
using Analytics.Core.Data.Entities;
using Analytics.Server.Api.Models;
using LinqToDB;

namespace Analytics.Server.Api.Controllers
{
	//[Route("api/raw-material")]
	public class RawMaterialController: ApiControllerBase
	{
		public RawMaterialModel Get(int id)
		{
			using (var storage = new Storage())
			{
				return storage
					.RawMaterials
					.LoadWith(rm => rm.RawMaterialType)
					.Where(m => m.RawMaterialId == id)
					.ToList()
					.Select(rm => new RawMaterialModel
					{
						ManufacturerId = rm.ManufacturerId,
						Name = rm.RawMaterialType.Name,
						RawMaterialId = rm.RawMaterialId,
						RawMaterialTypeId = rm.RawMaterialTypeId,
					})
					.SingleOrDefault();
			}
		}

		public IEnumerable<RawMaterialModel> Get()
		{
			using (var storage = new Storage())
			{
				return storage
					.RawMaterials
					.LoadWith(rm => rm.RawMaterialType)
					.ToList()
					.Select(rm => new RawMaterialModel
					{
						ManufacturerId = rm.ManufacturerId,
						Name = rm.RawMaterialType.Name,
						RawMaterialId = rm.RawMaterialId,
						RawMaterialTypeId = rm.RawMaterialTypeId,
					});
			}
		}

		public void Post([FromBody] RawMaterial material)
		{
			Argument.NotNull(material, "Material is required.");

			using (var storage = new Storage())
			{
				var existingMaterial = storage
					.RawMaterials
					.SingleOrDefault(rm => rm.RawMaterialId == material.RawMaterialId);

				if (null != existingMaterial)
				{
					storage.Update(material);
				}
				else
				{
					storage.Insert(material);
				}
			}
		}

		//[Route("~/api/raw-material")]
		//[HttpOptions]
		//[HttpDelete]
		public IHttpActionResult Delete(int id)
		{
			using (var storage = new Storage())
			{
				storage
					.RawMaterials
					.Delete(rm => rm.RawMaterialId == id);
			}

			return Ok();
		}
	}
}