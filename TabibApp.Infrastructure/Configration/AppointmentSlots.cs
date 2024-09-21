using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TabibApp.Infrastructure.Configration
{
    public class AppointmentSlots: IEntityTypeConfiguration<AppointmentSlot>
    {
        public void Configure(EntityTypeBuilder<AppointmentSlot> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.StartTime)
                .IsRequired();

            builder.Property(s => s.EndTime)
                .IsRequired();

            builder.HasOne(s => s.Clinic)
                .WithMany(c => c.AppointmentSlots)
                .HasForeignKey(s => s.ClinicId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(s => s.Doctor)
                .WithMany(d => d.AppointmentSlots)
                .HasForeignKey(s => s.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
    
}
