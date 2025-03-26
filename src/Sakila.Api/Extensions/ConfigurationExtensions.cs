namespace Sakila.Api.Extensions;

public static class ConfigurationExtensions
{
    public static void AddCustomConfiguration(this IConfigurationBuilder configurationBuilder, WebApplicationBuilder builder, string[] args)
    {
        string environmentName = builder.Environment.EnvironmentName;

        configurationBuilder.AddJsonFile("appsettings.json");
        configurationBuilder.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
        configurationBuilder.AddXmlFile("appsettings.xml");
        configurationBuilder.AddUserSecrets<Program>();
        configurationBuilder.AddCommandLine(args); // --NbpApiService:Table=B
        configurationBuilder.AddInMemoryCollection();
    }
}