using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TabibApp.Infrastructure.Configration
{
    public class ClinicAddressConfigartion : IEntityTypeConfiguration<ClinicAddress>
    {
     
        public void Configure(EntityTypeBuilder<ClinicAddress> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.BuildingNumber)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(a => a.StreetName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Floor)
                .HasMaxLength(5);

            builder.Property(a => a.ApartmentNumber)
                .HasMaxLength(10);

            builder.Property(a => a.Latitude)
                .IsRequired()
                .HasColumnType("decimal(10, 8)");

            builder.Property(a => a.Longitude)
                .IsRequired()
                .HasColumnType("decimal(11, 8)");
        }
    }
}
