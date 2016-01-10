using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Analytics.Core;
using Analytics.Core.Data;
using Analytics.Core.Data.Entities;
using LinqToDB;

namespace Analytics.Server.Api.Controllers
{
	//[Route("api/dict/material-type")]
	public class RawMaterialTypeController: ApiControllerBase
	{
		public RawMaterialType Get(int id)
		{
			using (var storage = new Storage())
			{
				return storage
					.RawMaterialTypes
					.SingleOrDefault(pc => pc.RawMaterialTypeId == id);
			}
		}

		public RawMaterialType Post([FromBody] RawMaterialType rawMaterialType)
		{
			Argument.NotNull(rawMaterialType, "Price extra category is required.");

			using (var storage = new Storage())
			{
				var existingMaterial = storage
					.RawMaterialTypes
					.SingleOrDefault(rmt => rmt.RawMaterialTypeId == rawMaterialType.RawMaterialTypeId);

				if (null != existingMaterial)
				{
					storage.Update(rawMaterialType);
				}
				else
				{
					storage.Insert(rawMaterialType);
				}

				return rawMaterialType;
			}
		}

		public IEnumerable<RawMaterialType> Get()
		{
			using (var storage = new Storage())
			{
				return storage.RawMaterialTypes.ToList();
			}
		}

		public void Delete(int id)
		{
			using (var storage = new Storage())
			{
				storage
					.RawMaterialTypes
					.Delete(pc => pc.RawMaterialTypeId == id);
			}
		}
	}
}