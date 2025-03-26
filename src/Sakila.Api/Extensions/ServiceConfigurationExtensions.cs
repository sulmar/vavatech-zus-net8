using Microsoft.AspNetCore.Http.Features;
using Sakila.Api.Services;
using System.Text.Json.Serialization;

namespace Sakila.Api.Extensions;

public static class ServiceConfigurationExtensions
{
    public static void AddCustomConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        // Konfiguracja NbpApiCurrencyServiceOptions
        services.Configure<NbpApiCurrencyServiceOptions>(configuration.GetSection("NbpApiService"));

        // Konfiguracja JSON SerializerOptions
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            // options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // Zawiera referencje zamiast zapętlonych struktur
            // options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Zawiera wartość null zamiast zapętlonych struktur
        });

        // Konfiguracja FormOptions
        services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 1 * 1024 * 1024; // 1 MB
        });
    }
}