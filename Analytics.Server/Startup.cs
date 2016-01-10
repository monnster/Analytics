using System.ComponentModel;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using Analytics.Server.Infrastructure;
using Analytics.Server.Infrastructure.Filters;
using Analytics.Server.Infrastructure.Handlers;
using LightInject;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Analytics.Server
{
	public class Startup
	{
		public void Configuration(IAppBuilder builder)
		{
			var config = new HttpConfiguration();

			ConfigureFormatters(config);
			ConfigureHandlers(config);

			Container.Instance.EnableWebApi(config);
			Container.Instance.RegisterApiControllers();

			ConfigureRoutes(config);

			config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

			config.EnsureInitialized();

			builder.UseWebApi(config);
		}

		private void ConfigureHandlers(HttpConfiguration config)
		{
			//config.MessageHandlers.Add(new CustomAuthenticationHandler());
			config.MessageHandlers.Add(new CrossDomainRequestHandler());
			config.MessageHandlers.Add(new RequestTraceHandler());
			config.MessageHandlers.Add(new ProfilingHandler());

			config.Filters.Add(new LogExceptionAttribute());
		}

		private void ConfigureFormatters(HttpConfiguration config)
		{
			JsonMediaTypeFormatter json = config.Formatters.JsonFormatter;
			json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			json.SerializerSettings.Formatting = Formatting.Indented;

			var converters = json.SerializerSettings.Converters;
			converters.Add(new StringEnumConverter());
		}

		private void ConfigureRoutes(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultRoute",
				routeTemplate: "api/{controller}/{id}/{action}",
				defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional }
			);
		}
	}
}