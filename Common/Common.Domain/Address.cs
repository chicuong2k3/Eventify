namespace Common.Domain;

public record Address : Location
{
    public Address(string? street, string ward, string district, string province, string country)
        : base(street, ward, district, province, country)
    {
    }
}
