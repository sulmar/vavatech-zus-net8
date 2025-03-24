using Microsoft.Extensions.Options;
using Sakila.Api.Domain.Abstractions;

namespace Sakila.Api.Services;

public class NbpApiCurrencyServiceOptions
{
    public string Url { get; set; }
    public string Table { get; set; }
    public string Symbol { get; set; }
}


public class NbpApiCurrencyService : ICurrencyService
{
    private NbpApiCurrencyServiceOptions _options;

    public NbpApiCurrencyService(IOptions<NbpApiCurrencyServiceOptions> options)
    {
        _options = options.Value;
    }

    // https://api.nbp.pl/api/exchangerates/rates/A/EUR?format=json
    public decimal GetCurrencyRatio(string symbol)
    {
        string url = $"{_options.Url}/api/exchangerates/rates/{_options.Table}/{symbol}?format=json";

        throw new NotImplementedException();
    }
}
