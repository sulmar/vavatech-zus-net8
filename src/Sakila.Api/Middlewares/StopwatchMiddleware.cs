using System.Diagnostics;

namespace Sakila.Api.Middlewares;


public static partial class StopwatchMiddlewareExtensions
{
    public static IApplicationBuilder UseStopwatch(this IApplicationBuilder app)
    {
        app.UseMiddleware<StopwatchMiddleware>();

        return app;
    }
}
public static partial class StopwatchMiddlewareExtensions
{
    public class StopwatchMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<StopwatchMiddleware> logger;

        public StopwatchMiddleware(RequestDelegate next, ILogger<StopwatchMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Przed wykonaniem endpointu
            var stopwatch = Stopwatch.StartNew();

            await next(context);

            // Po wykonaniu endpointu

            stopwatch.Stop();

            logger.LogInformation("Czas wykonania: {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
        }
    }
}