namespace Common.Domain;

public record CurrencyDetails
{
    public string CurrencyCode { get; } = default!;
    public bool InUse { get; }
    public int DecimalPlaces { get; }

    private static readonly CurrencyDetails none = new(nameof(None), false, 0);
    public static CurrencyDetails None => none;

    private CurrencyDetails() { }

    public CurrencyDetails(string currencyCode, bool inUse, int decimalPlaces)
    {
        CurrencyCode = currencyCode;
        InUse = inUse;
        DecimalPlaces = decimalPlaces;
    }
}