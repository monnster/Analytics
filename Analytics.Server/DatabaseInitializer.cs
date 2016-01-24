using System.Collections.Generic;
using System.Linq;
using Analytics.Core.Data;
using Analytics.Core.Data.Entities;
using Analytics.Core.Data.Enums;
using LinqToDB;

namespace Analytics.Server
{
	public class DatabaseInitializer
	{
		public void Seed(Storage storage, int plantCode)
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

			var allManufacturers = storage
				.Manufacturers
				.ToList();

			var primaryManufacturers = allManufacturers.Where(m => m.IsPrimary);
			var nonPrimaryManufacturers = allManufacturers.Where(m => !m.IsPrimary);

			var materialThickness = EnumerateThickness(plantCode);

			foreach (var thickness in materialThickness)
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

						var materialTypeId = storage.InsertWithIdentity(materialType);

						foreach (var primaryManufacturer in primaryManufacturers)
						{
							var rawMaterialId = storage.InsertWithIdentity(
								new RawMaterial
								{
									ManufacturerId = primaryManufacturer.ManufacturerId,
									RawMaterialTypeId = (int)(long) materialTypeId,
								});

							foreach (var productName in EnumerateProductNames(plantCode, rollType, alloyType))
							{
								storage.Insert(
									new Product
									{
										ManufacturerId = primaryManufacturer.ManufacturerId,
										RawMaterialId = (int)(long)rawMaterialId,
										Name = productName,
										Thickness = thickness,
									});


								foreach (var nonPrimaryManufacturer in nonPrimaryManufacturers)
								{
									storage.Insert(
										new Product
										{
											ManufacturerId = nonPrimaryManufacturer.ManufacturerId,
											RawMaterialId = (int)(long)rawMaterialId,
											Name = productName,
											Thickness = thickness,
										});
								}
								
							}
						}
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

		private IEnumerable<decimal> EnumerateThickness(int plantCode)
		{
			switch (plantCode)
			{
				case 1:
					return new[]
					{
						3.0m,
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

				case 2:
					return new[]
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
					};

				default:
					return Enumerable.Empty<decimal>();
			}
		}

		private IEnumerable<string> EnumerateProductNames(int plantCode, RollType rollType, AlloyType alloyType)
		{
			if (plantCode == 1)
			{
				if (rollType == RollType.Hot)
				{
					return new[]
					{
						"150х100х",
						"100х100х",
						"120х80х",
						"120х120х",
						"140х100х",
						"160х80х",
						"140х140х",
						"160х120х",
						"150х150х",
						"160х140х",
						"180х100х",
						"200х100х",
						"160х160х",
						"180х140х",
						"200х120х",
						"180х180х",
						"200х160х",
						"240х120х",
						"200х200х",
						"240х160х",
						"250х150х",
						"300х100х",
						"250х250х",
						"300х200х",
						"300х300х",
						"159х",
						"219х",
						"273х",
						"325 - 377х",
						"426х",
					};
				}
			}

			if (plantCode == 2)
			{
				if (rollType == RollType.Hot)
				{
					return new[]
					{
						"г/к 15х15х",
						"г/к 20х10х",
						"г/к 20х20х",
						"г/к 25х25х",
						"г/к 30х20х",
						"г/к 30х30х",
						"г/к 40х20х",
						"г/к 40х25х",
						"г/к 40х40х",
						"г/к 50х25х",
						"г/к 50х30х",
						"г/к 50х50х",
						"г/к 60х30х",
						"г/к 60х40х",
						"г/к 60х60х",
						"г/к 80х40х",
						"г/к 16х",
						"г/к 20х",
						"г/к 25х",
						"г/к 28х",
						"г/к 30х",
						"г/к 32х",
						"г/к 38х",
						"г/к 40х",
						"г/к 42х",
						"г/к 48х",
						"г/к 51х",
						"г/к 57х",
						"г/к 60х",
						"г/к 76х",
					};
				}
				if (rollType == RollType.Cold)
				{
					return new[]
					{
						"х/к 15х15х",
						"х/к 20х10х",
						"х/к 25х25х",
						"х/к 30х15х",
						"х/к 30х30х",
						"х/к 40х20х",
						"х/к 50х25х",
						"х/к 60х30х",
						"х/к 16х",
						"х/к 18х",
						"х/к 22х",
						"х/к 25х",
						"х/к 30х",
						"х/к 40х",
						"х/к 51х",
						"х/к 76х",
					};
				}
			}

			return Enumerable.Empty<string>();
		}
	}
}
 
 