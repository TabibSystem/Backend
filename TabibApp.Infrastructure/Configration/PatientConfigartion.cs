using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TabibApp.Infrastructure.Configration
{
       public class PatientConfigartion : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
               builder.Property(p => p.Age).IsRequired();

            builder.HasOne(p => p.ApplicationUser)
                   .WithOne(u => u.Patient)
                   .HasForeignKey<Patient>(p => p.ApplicationUserId);

           
            builder.HasMany(p => p.MedicalHistory)
                   .WithOne(m => m.Patient)
                   .HasForeignKey(m => m.PatientId);

            builder.HasMany(p => p.Appointments)
                   .WithOne(a => a.Patient)
                   .HasForeignKey(a => a.PatientId);

            builder.HasMany(p => p.Prescriptions)
                   .WithOne(pr => pr.Patient)
                   .HasForeignKey(pr => pr.PatientId);

        
            builder.HasMany(p => p.doctors) 
                   .WithMany(d => d.Patients);



        }
    }
}
