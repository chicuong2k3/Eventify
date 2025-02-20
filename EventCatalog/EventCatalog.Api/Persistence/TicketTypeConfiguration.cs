using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventCatalog.Api.Persistence;

internal sealed class TicketTypeConfiguration : IEntityTypeConfiguration<TicketType>
{
    public void Configure(EntityTypeBuilder<TicketType> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.OwnsOne(t => t.Price, price =>
        {
            price.Property(p => p.Amount)
                .HasColumnName("Price")
                .IsRequired();

            price.OwnsOne(p => p.Currency, currency =>
            {
                currency.Property(c => c.CurrencyCode)
                    .HasColumnName("CurrencyCode")
                    .IsRequired()
                    .HasMaxLength(3);
            });

        });

    }
}
