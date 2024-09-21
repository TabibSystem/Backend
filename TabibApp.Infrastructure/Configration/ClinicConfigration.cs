using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TabibApp.Infrastructure.Configration
{
    public class ClinicConfigration : IEntityTypeConfiguration<Clinic>
    {
        public void Configure(EntityTypeBuilder<Clinic> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.PhoneNumber)
                .HasMaxLength(15);


            builder.HasOne(c => c.OwnerDoctor)
                .WithMany(d => d.Clinics)
                .HasForeignKey(c => c.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Governorate)
                .WithMany(g => g.Clinics)
                .HasForeignKey(c => c.GovernorateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.ClinicAddress)
                .WithOne(a => a.Clinic)
                .HasForeignKey<ClinicAddress>(a => a.ClinicId)
                .OnDelete(DeleteBehavior.Cascade);
        }

       
    }
}
