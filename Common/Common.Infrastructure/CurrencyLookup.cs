namespace Common.Domain;

internal class CurrencyLookup : ICurrencyLookup
{
    private readonly Dictionary<string, CurrencyDetails> currencies;

    public CurrencyLookup()
    {
        currencies = new Dictionary<string, CurrencyDetails>(StringComparer.OrdinalIgnoreCase)
        {
            { "USD", new CurrencyDetails("USD", true, 2) },
            { "EUR", new CurrencyDetails("EUR", true, 2) },
            { "JPY", new CurrencyDetails("JPY", true, 0) },
            { "GBP", new CurrencyDetails("GBP", true, 2) },
            { "AUD", new CurrencyDetails("AUD", true, 2) }
        };
    }

    public CurrencyDetails FindCurrency(string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            throw new ArgumentException("Currency code cannot be null or empty.", nameof(currencyCode));
        }

        if (currencies.TryGetValue(currencyCode, out var currency))
        {
            return currency;
        }

        return CurrencyDetails.None;
    }
}
