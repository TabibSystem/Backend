using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace TabibApp.Infrastructure.Configration
{
    public class AppointmentConfigration: IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.AppointmentDate)
                .IsRequired();

            builder.Property(a => a.Status)
                .HasConversion<int>(); 

            builder.HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.AppointmentSlot)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.AppointmentSlotId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Prescription)
                .WithOne(p => p.Appointment)
                .HasForeignKey<Appointment>(a => a.PrescriptionId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
    
}
