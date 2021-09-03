namespace ConformityCheck.Web.Filters
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var logger = (ILogger<ExceptionFilter>)context.HttpContext.RequestServices.GetService(typeof(ILogger<ExceptionFilter>));
            var requestId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;

            logger.LogError($"Exception in Request ID: {requestId}");

            base.OnException(context);
        }
    }
}
