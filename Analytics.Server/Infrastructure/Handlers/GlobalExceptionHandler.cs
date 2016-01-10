using System.Net;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Analytics.Core;
using NLog;

namespace Analytics.Server.Infrastructure.Handlers
{
	public class GlobalExceptionHandler : ExceptionHandlerBase
	{
		private Logger _logger = LogManager.GetLogger("ApiGlobal");

		public override void HandleCore(ExceptionHandlerContext context)
		{
			var request = context.Request.Content.ReadAsStringAsync().Result;

			_logger.Fatal(context.Exception, "Error processing request {0}:".Fill(request));

			context.Result = new StatusCodeResult(HttpStatusCode.InternalServerError, context.Request);

			base.HandleCore(context);
		}
	}

}