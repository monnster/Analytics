using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Analytics.Core.Utils;
using Microsoft.Owin;
using NLog;

namespace Analytics.Server.Infrastructure.Handlers
{
	public class RequestTraceHandler : DelegatingHandler
	{
		private Logger _logger = LogManager.GetLogger("ApiRequestTracer");

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var content = await request.Content.ReadAsStringAsync();

			var requestId = UidTool.GetTicksHex();

			_logger.Debug("[{0}] {1} {2} {3}\r\n{4}",
				requestId,
				GetClientIp(request),
				request.Method,
				request.RequestUri,
				content);


			return await base.SendAsync(request, cancellationToken);
		}

		private string GetClientIp(HttpRequestMessage request)
		{
			if (request == null)
			{
				return null;
			}

			if (request.Properties.ContainsKey("MS_OwinContext"))
			{
				return ((OwinContext)request.Properties["MS_OwinContext"]).Request.RemoteIpAddress;
			}
			return null;
		}
	}

}