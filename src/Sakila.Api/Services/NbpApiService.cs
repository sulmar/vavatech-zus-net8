using Microsoft.Extensions.Options;
using Sakila.Api.Domain.Abstractions;

namespace Sakila.Api.Services;

public class NbpApiCurrencyServiceOptions
{
    public string Url { get; set; }
    public string Table { get; set; }
    public string Symbol { get; set; }
}

public class ForexApiCurrencyService : ICurrencyService
{
    public decimal GetCurrencyRatio(string symbol)
    {
        throw new NotImplementedException();
    }
}

public class NbpApiCurrencyService : ICurrencyService
{
    private readonly ILogger<NbpApiCurrencyService> logger;
    private NbpApiCurrencyServiceOptions _options;

    public NbpApiCurrencyService(ILogger<NbpApiCurrencyService> logger,  IOptions<NbpApiCurrencyServiceOptions> options)
    {
        _options = options.Value;
        this.logger = logger;
    }

    // https://api.nbp.pl/api/exchangerates/rates/A/EUR?format=json
    public decimal GetCurrencyRatio(string symbol)
    {

        string url = $"{_options.Url}/api/exchangerates/rates/{_options.Table}/{symbol}?format=json";

        decimal ratio = Math.Round( 4 + (decimal) Random.Shared.NextDouble(), 2);

        logger.LogInformation("GetCurrencyRatio symbol: {symbol} ratio: {ratio}", symbol, ratio);

        return ratio;
    }
}
