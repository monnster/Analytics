using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using Analytics.Core;
using Analytics.Core.Utils;
using NLog;

namespace Analytics.Server.Infrastructure.Filters
{

	public class LogExceptionAttribute : ExceptionFilterAttribute
	{
		private Logger _logger = LogManager.GetLogger("ApiGlobal");

		public override void OnException(HttpActionExecutedContext context)
		{
			if (null == context.Exception)
				return;

			var httpResultException = context.Exception as HttpResponseException;

			if (null == httpResultException)
			{
				var exceptionId = UidTool.Next();
				context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, new
				{
					Message = context.Exception.Message,
					Details = "See log entry identified by token {0}".Fill(exceptionId),
					Code = 500,
				});
				_logger.Error("WEBAPI 500, token: {0}".Fill(exceptionId), context.Exception);
			}
			else
			{
				_logger.Error("Unhandled http exception", context.Exception);
			}
		}
	}

}