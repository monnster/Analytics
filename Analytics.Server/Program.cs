using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analytics.Core;
using Analytics.Core.Data;
using Analytics.Core.Data.Entities;
using Analytics.Core.Data.Enums;
using Analytics.Core.Utils;
using LinqToDB;
using Microsoft.Owin.Hosting;

namespace Analytics.Server
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
				args = new[] { "--info" };

			switch (args[0])
			{
				case "--init":
					Init();
					Console.WriteLine("Database initialized.");
					break;

				case "--run":
					StartServer();
					break;

				default:
					Console.WriteLine("Usage keys:");
					Console.WriteLine("--init                - initializes data");
					Console.WriteLine("--run			         - starts server on {0} endpoint".Fill(GetServerAddress()));
					break;
			}

		}

		private static void Init()
		{
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

				new DatabaseInitializer().Seed(storage);
			}
		}

		private static void StartServer()
		{
			var serverUrl = GetServerAddress();

			Console.WriteLine("Starting server at {0}", serverUrl);
			using (WebApp.Start<Startup>(serverUrl))
			{
				Console.WriteLine("Server started. Press <Enter> or any other key to stop.");

				Console.ReadKey();
			}
		}


		private static string GetServerAddress()
		{
			dynamic config = FluentConfiguration.Instance;
			int defaultPort = config.Server.DefaultPort;
			string defaultProtocol = config.Server.DefaultProtocol;
			string defaultHost = config.Server.DefaultHost;

			return "{0}://{1}:{2}".Fill(defaultProtocol, defaultHost, defaultPort);
		}
	}
}
