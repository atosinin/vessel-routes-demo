using DotNetApi.Localization;
using Microsoft.Extensions.Localization;
using System.Net;

namespace DotNetApi.Helpers.Exceptions
{
    public class VesselRoutesDemoExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStringLocalizer<SharedLocalizer> _sharedLocalizer;

        public VesselRoutesDemoExceptionMiddleware(
            RequestDelegate next, 
            IStringLocalizer<SharedLocalizer> sharedLocalizer)
        {
            _next = next;
            _sharedLocalizer = sharedLocalizer;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode;
            string message = string.Empty;
            switch (exception)
            {
                case UserMessageOnlyException:
                    // Do not log
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = _sharedLocalizer[exception.Message];
                    break;
                case UserMessageLoggedException:
                    Serilog.Log.Error(
                        exception,
                        "Raised by '{User}' at '{IpAddress}'", 
                        context.User.Identity?.Name ?? "Anonymous",
                        context.Connection.RemoteIpAddress
                    );
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = _sharedLocalizer[exception.Message];
                    break;
                case BadRequestException:
                    Serilog.Log.Error(
                        exception,
                        "Raised by '{User}' at '{IpAddress}'",
                        context.User.Identity?.Name ?? "Anonymous",
                        context.Connection.RemoteIpAddress
                    );
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = _sharedLocalizer["Bad request"];
                    break;
                default:
                    Serilog.Log.Error(
                        exception,
                        "Raised by '{User}' at '{IpAddress}'",
                        context.User.Identity?.Name ?? "Anonymous",
                        context.Connection.RemoteIpAddress
                    );
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            context.Response.StatusCode = statusCode;
            if (!string.IsNullOrWhiteSpace(message))
            {
                context.Response.ContentType = "application/json";
                ErrorModel errorModel = new()
                {
                    Message = message
                };
                await context.Response.WriteAsync(errorModel.ToJsonString());
            }
        }
    }

    public static class VesselRoutesDemoExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseVesselRoutesDemoException(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<VesselRoutesDemoExceptionMiddleware>();
        }
    }
}
