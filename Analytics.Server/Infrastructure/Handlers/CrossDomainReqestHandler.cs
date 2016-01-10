using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Analytics.Server.Infrastructure.Handlers
{
	/// <summary>
	/// This handler responds to OPTIONS request, allowing cross-domain requests and POST method only.
	/// </summary>
	public class CrossDomainRequestHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			HttpResponseMessage message;

			if (request.Method == HttpMethod.Options)
			{
				message = request.CreateResponse(HttpStatusCode.OK);
			}
			else
			{
				message = await base.SendAsync(request, cancellationToken);
			}

			message.Headers.Add("Access-Control-Allow-Origin", "*");
			message.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, X-Auth-Signature, X-Auth-Login, X-Auth-Method, Authorization");
			message.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE");

			return message;
		}
	}

}