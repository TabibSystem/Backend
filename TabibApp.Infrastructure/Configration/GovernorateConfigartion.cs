using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TabibApp.Infrastructure.Configration
{
    public class GovernorateConfigartion : IEntityTypeConfiguration<Governorate>
    {
        public void Configure(EntityTypeBuilder<Governorate> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(g => g.Doctors)
                .WithOne(d => d.Governorate)
                .HasForeignKey(d => d.GovernorateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(g => g.Clinics)
                .WithOne(c => c.Governorate)
                .HasForeignKey(c => c.GovernorateId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
