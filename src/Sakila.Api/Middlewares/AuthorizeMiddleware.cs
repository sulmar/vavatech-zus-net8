namespace Sakila.Api.Middlewares;

public static class AuthorizeMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthorize(this IApplicationBuilder app)
    {
        app.UseMiddleware<AuthorizeMiddleware>();

        return app;
    }
}

public class AuthorizeMiddleware
{
    private readonly RequestDelegate next;

    public AuthorizeMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authorization = context.Request.Headers["Authorization"];

        string accessToken = "abc";

        if (authorization.ToString() != $"Bearer {accessToken}")
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            return;
        }

        await next(context);
    }
}