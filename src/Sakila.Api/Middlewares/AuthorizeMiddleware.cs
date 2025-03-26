namespace Sakila.Api.Middlewares;

public class AuthorizeMiddleware
{
    private readonly RequestDelegate next;

    public AuthorizeMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authorizeSecretKey = context.Request.Headers["x-secret-key"];

        if (authorizeSecretKey.ToString() != "abc")
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            return;
        }

        await next(context);
    }
}