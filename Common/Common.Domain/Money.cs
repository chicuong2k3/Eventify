using System.Globalization;

namespace Common.Domain;

public record Money
{
    private const string DefaultCurrency = "VND";
    public decimal Amount { get; private set; }
    public CurrencyDetails Currency { get; private set; } = default!;

    public static Money GetZeroMoney(CurrencyDetails currency)
    {
        return new Money
        {
            Amount = 0,
            Currency = currency
        };
    }


    protected Money() { }

    private Money(
        ICurrencyLookup currencyLookup,
        decimal amount,
        string currencyCode = DefaultCurrency)
    {
        if (amount < 0)
            throw new ValidationException("Money amount cannot be negative");

        if (string.IsNullOrWhiteSpace(currencyCode))
            throw new ValidationException("Currency code cannot be null or empty");

        var currency = currencyLookup.FindCurrency(currencyCode);
        if (!currency.InUse)
            throw new ValidationException($"Currency {currencyCode} is not valid");

        if (decimal.Round(amount, currency.DecimalPlaces) != amount)
            throw new ValidationException($"Amount in {currencyCode} cannot have more than {currency.DecimalPlaces} decimals");

        Amount = amount;
        Currency = currency;
    }

    public static Money FromDecimal(
        ICurrencyLookup currencyLookup,
        decimal amount,
        string currencyCode = DefaultCurrency)
        => new Money(currencyLookup, amount, currencyCode);

    public static Money FromString(
        ICurrencyLookup currencyLookup,
        string amount,
        string currencyCode = DefaultCurrency)
    {
        if (!decimal.TryParse(amount, NumberStyles.Currency, CultureInfo.InvariantCulture, out var parsedAmount))
        {
            throw new ValidationException("Invalid amount.");
        }

        return new Money(currencyLookup, parsedAmount, currencyCode);
    }

    public override string ToString() => $"{Amount} {Currency.CurrencyCode}";

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new CurrencyMismatchException("Cannot add money with different currencies.");
        }

        return left with { Amount = left.Amount + right.Amount };
    }

    public static Money operator -(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new CurrencyMismatchException("Cannot subtract money with different currencies.");
        }

        return left with { Amount = left.Amount - right.Amount };
    }

    public static Money operator *(Money left, decimal right)
    {
        return left with { Amount = left.Amount * right };
    }
}

public class CurrencyMismatchException : Exception
{
    public CurrencyMismatchException(string message) :
      base(message)
    {
    }
}