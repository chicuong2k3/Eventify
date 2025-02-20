namespace Common.Domain;

public record Location
{
    private Location()
    {
    }

    public string? Street { get; }
    public string Ward { get; }
    public string District { get; }
    public string Province { get; }
    public string Country { get; }

    public Location(string? street, string ward, string district, string province, string country)
    {
        if (string.IsNullOrEmpty(ward))
            throw new ValidationException("Ward is required.");
        if (string.IsNullOrEmpty(district))
            throw new ValidationException("District is required.");
        if (string.IsNullOrEmpty(province))
            throw new ValidationException("Province is required.");
        if (string.IsNullOrEmpty(country))
            throw new ValidationException("Country is required.");

        Street = street;
        Ward = ward;
        District = district;
        Province = province;
        Country = country;
    }
}
