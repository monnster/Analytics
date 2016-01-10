using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Analytics.Server.Infrastructure.Handlers
{
	public class ProfilingHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			var watcher = Stopwatch.StartNew();
			var response = await base.SendAsync(request, cancellationToken);
			watcher.Stop();

			//store duration somewheren here in the response header
			response.Headers.Add("X-Duration", watcher.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture));

			return response;
		}
	}

}