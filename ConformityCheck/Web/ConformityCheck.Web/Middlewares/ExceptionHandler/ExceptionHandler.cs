namespace ConformityCheck.Web.Middlewares.ExceptionHandler
{
    using System.Net;

    using ConformityCheck.Web.ViewModels;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    // not used
    public static class ExceptionHandler
    {
        public static void UseApiExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    // if any exception then report it and log it
                    if (contextFeature != null)
                    {
                        // Technical Exception for troubleshooting
                        var logger = loggerFactory.CreateLogger("GlobalException");
                        logger.LogError($"Something went wrong: {contextFeature.Error}");

                        // Business exception - exit gracefully
                        await context.Response.WriteAsync(new ErrorViewModel()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Something went wrongs.Please try again later",
                        }.ToString());
                    }
                });
            });
        }
    }
}
