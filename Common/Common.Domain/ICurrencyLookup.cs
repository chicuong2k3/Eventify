namespace Common.Domain;

public interface ICurrencyLookup
{
    CurrencyDetails FindCurrency(string currencyCode);
}
