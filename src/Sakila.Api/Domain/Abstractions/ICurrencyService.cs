namespace Sakila.Api.Domain.Abstractions;

public interface ICurrencyService
{
    decimal GetCurrencyRatio(string symbol);
}
