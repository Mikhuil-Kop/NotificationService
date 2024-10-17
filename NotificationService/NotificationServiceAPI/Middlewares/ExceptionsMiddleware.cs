﻿using NotificationCommonLibrary.Exceptions;

namespace NotificationServiceAPI.Middlewares
{
    public class ExceptionsMiddleware
    {
        private RequestDelegate Next { get; }
        private ILogger Logger { get; }

        public ExceptionsMiddleware(RequestDelegate next, ILogger<ExceptionsMiddleware> logger)
        {
            Next = next;
            Logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (CustomForbiddenExceiption e)
            {
                Logger.LogError(e, "Error processing request: {Path}", context.Request.Path);

                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error processing request: {Path}", context.Request.Path);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(e.Message);
            }
        }
    }
}

