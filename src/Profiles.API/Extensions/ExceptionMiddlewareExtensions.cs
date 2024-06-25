using Microsoft.AspNetCore.Diagnostics;
using Profiles.Domain.ErrorModel;

namespace Profiles.API.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app, Serilog.ILogger logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature is not null)
                {
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        _ => StatusCodes.Status500InternalServerError
                    };

                    logger.Error($"Something went wrong: {contextFeature.Error}");

                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message,
                    }.ToString());
                }
            });
        });
    }
}
 