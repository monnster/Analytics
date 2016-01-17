using Analytics.Core.Data;
using Analytics.Core.Data.Entities;
using Analytics.Core.Data.Enums;
using LinqToDB;

namespace Analytics.Server
{
	public class DatabaseInitializer
	{
		public void Seed(Storage storage)
		{
			storage.Insert(new Manufacturer { Name = "ЧерМК", IsPrimary = true });
			storage.Insert(new Manufacturer { Name = "НЛМК", IsPrimary = true });
			storage.Insert(new Manufacturer { Name = "ММК", IsPrimary = true });
			storage.Insert(new Manufacturer { Name = "(1) КТЗ", IsPrimary = false });
			storage.Insert(new Manufacturer { Name = "(1) РязТЗ", IsPrimary = false });
			storage.Insert(new Manufacturer { Name = "(1) БТЗ", IsPrimary = false });
			storage.Insert(new Manufacturer { Name = "(1) ОМК", IsPrimary = false });
			storage.Insert(new Manufacturer { Name = "(1) СЗТЗ", IsPrimary = false });
			storage.Insert(new Manufacturer { Name = "(1) ТПК Союз", IsPrimary = false });
			storage.Insert(new Manufacturer { Name = "(1) Машпрофиль", IsPrimary = false });
			storage.Insert(new Manufacturer { Name = "(1) Профиль А", IsPrimary = false });
			storage.Insert(new Manufacturer { Name = "(1) БИС", IsPrimary = false });

			storage.Insert(new Manufacturer { Name = "(2) ВестМет", IsPrimary = false });
			storage.Insert(new Manufacturer { Name = "(2) Промизделия", IsPrimary = false });
			storage.Insert(new Manufacturer { Name = "(2) ТЭМПО", IsPrimary = false });
			storage.Insert(new Manufacturer { Name = "(2) ОМК", IsPrimary = false });
			storage.Insert(new Manufacturer { Name = "(2) ТМК", IsPrimary = false });
			storage.Insert(new Manufacturer { Name = "(2) Уралтрубпром", IsPrimary = false });
			storage.Insert(new Manufacturer { Name = "(2) Исаевский ТЗ", IsPrimary = false });




			storage.Insert(new PriceExtraCategory { Name = "Себестоимость" });
			storage.Insert(new PriceExtraCategory { Name = "Доставка материала" });
			storage.Insert(new PriceExtraCategory { Name = "Передел" });
			storage.Insert(new PriceExtraCategory { Name = "Доставка до Москвы" });

			var regularMaterialThickness = new[]
			{
					1.0m,
					1.2m,
					1.35m,
					1.5m,
					1.75m,
					1.8m,
					2.0m,
					2.5m,
					3.0m,
					3.5m,
					4.0m,
					5.0m,
					6.0m,
					7.0m,
					8.0m,
					9.0m,
					10.0m,
					12.0m,
					16.0m,
				};

			foreach (var thickness in regularMaterialThickness)
			{
				foreach (var rollType in new [] {RollType.Cold, RollType.Hot })
				{
					foreach (var alloyType in new[] {AlloyType.LowAlloy, AlloyType.Regular })
					{
						var materialType = new RawMaterialType
						{
							AlloyType = alloyType,
							RollType = rollType,
							Thickness = thickness,
						};

						SetMaterialTypeName(materialType);

						storage.Insert(materialType);
					}
				}
			}
		}

		private void SetMaterialTypeName(RawMaterialType rawMaterialType)
		{
			string rollType, alloyType;
			switch (rawMaterialType.RollType)
			{
				case RollType.Cold:
					rollType = " х/к";
					break;
				
				case RollType.Hot:
					rollType = " г/к";
					break;

				default:
					rollType = "";
					break;
			}

			switch (rawMaterialType.AlloyType)
			{
				case AlloyType.LowAlloy:
					alloyType = " низколегир.";
					break;

				case AlloyType.Regular:
					alloyType = " рядовой";
					break;

				default:
					alloyType = "";
					break;

			}

			rawMaterialType.Name = $"Штрипс{alloyType}{rollType} {rawMaterialType.Thickness}мм";
		}
	}
}
 
 