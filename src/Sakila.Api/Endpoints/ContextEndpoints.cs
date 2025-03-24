namespace Sakila.Api.Endpoints;

public static class ContextEndpoints
{
    public static RouteGroupBuilder MapRootApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", () => "Hello, World!");
        
        var homeHandler = () => "Hello, World!";

        group.MapGet("/home", homeHandler);


        group.Map("/context", (HttpContext context) =>
        {
            HttpRequest request = context.Request;

            var requestMethod = context.Request.Method;
            var requestPath = context.Request.Path;
            var id = context.Request.Query["id"];
            var name = context.Request.Query["name"];

            HttpResponse response = context.Response;
        });

        group.MapGet("/hello", (HttpRequest req, HttpResponse res) => Results.Ok("Hello World!"));

        return group;
    }
}