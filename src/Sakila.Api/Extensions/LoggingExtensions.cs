using Serilog.Formatting.Compact;
using Serilog;
using FluentValidation;
using Sakila.Api.Domain.Models;
using Sakila.Api.Mappers;
using Sakila.Api.Validators;

namespace Sakila.Api.Extensions;

public static class LoggingExtensions
{
    public static void AddSerilogLogging(this ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.ClearProviders(); // Usunięcie domyślnych providerów

        var logger = new LoggerConfiguration()
            .WriteTo.Console()              // Serilog.Sinks.Console
            .WriteTo.File("log.txt")   //Serilog.Sinks.File
            .WriteTo.File(new CompactJsonFormatter(), "log.json") // Serilog.Formatting.Compact
            .CreateLogger();

        // dotnet add package Serilog.Extensions.Logging
        loggingBuilder.AddSerilog(logger);
    }
}


