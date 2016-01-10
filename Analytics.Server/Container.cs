using Analytics.Core;
using LightInject;

namespace Analytics.Server
{
	public class Container
	{
		private static readonly IServiceContainer _instance;

		public static IServiceContainer Instance
		{
			get { return _instance; }
		}

		static Container()
		{
			_instance = new ServiceContainer(new ContainerOptions { EnableVariance = false });
			_instance.RegisterAssembly(typeof(Argument).Assembly);
			_instance.RegisterAssembly(typeof(Container).Assembly);
		}

	}
}