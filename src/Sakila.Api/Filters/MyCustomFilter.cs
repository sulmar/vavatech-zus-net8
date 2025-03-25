
using System.Diagnostics;

namespace Sakila.Api.Filters;

public class MyCustomFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        Console.WriteLine("Logika przez wykonaniem endpointu...");

        var result = await next(context);

        Console.WriteLine("Logika po wykonaniu endpointu.");

        return result;
    }
}


public class StopwatchFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {

        var stopwatch = Stopwatch.StartNew();

        var result = await next(context);

        stopwatch.Stop();

        Console.WriteLine($"Czas wykonania: {stopwatch.ElapsedMilliseconds} ms");

        return result;

    }
}