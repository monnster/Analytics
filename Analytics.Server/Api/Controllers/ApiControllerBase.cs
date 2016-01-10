using System.Net;
using System.Net.Http;
using System.Web.Http;
using Analytics.Server.Infrastructure.Filters;

namespace Analytics.Server.Api.Controllers
{
	[LogException]
	public abstract class ApiControllerBase : ApiController
	{
		protected HttpResponseException HttpException(HttpStatusCode httpCode, string message)
		{
			return new HttpResponseException(
				Request.CreateResponse(httpCode, new 
				{
					Message = message,
					Code = (int)httpCode,
				}));
		}
	}

}