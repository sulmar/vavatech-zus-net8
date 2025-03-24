using Sakila.Api.Endpoints;

namespace Sakila.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapApi(this WebApplication app)
    {
        app.MapGroup("/").MapRootApi();
        app.MapGroup("/customers").MapCustomersApi();
        app.MapGroup("/orders").MapOrdersApi();
        app.MapGroup("/products").MapProductsApi();

        return app;

    }
}
