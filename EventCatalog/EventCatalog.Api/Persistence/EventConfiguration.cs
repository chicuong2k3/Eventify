using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventCatalog.Api.Persistence;

internal sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(e => e.Description)
            .IsRequired();

        builder.OwnsOne(e => e.Schedule, schedule =>
        {
            schedule.Property(s => s.Start)
                .HasColumnName("StartAt")
                .IsRequired();

            schedule.Property(s => s.End)
                    .HasColumnName("EndAt");
        });

        builder.OwnsOne(e => e.Location, location =>
        {
            location.Property(l => l.Street)
                .HasColumnName("Street")
                .HasMaxLength(100);

            location.Property(l => l.District)
                .HasColumnName("District")
                .IsRequired()
                .HasMaxLength(100);

            location.Property(l => l.Province)
                .HasColumnName("Province")
                .IsRequired()
                .HasMaxLength(100);

            location.Property(l => l.Country)
                .HasColumnName("Country")
                .IsRequired()
                .HasMaxLength(100);

            location.Property(l => l.Province)
                .HasColumnName("Province")
                .IsRequired()
                .HasMaxLength(100);
        });

        builder.Property(e => e.Status)
            .HasConversion(builder => builder.ToString(), value => Enum.Parse<EventStatus>(value));

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(e => e.CategoryId);

        builder.HasMany(e => e.TicketTypes)
            .WithMany();
    }
}
