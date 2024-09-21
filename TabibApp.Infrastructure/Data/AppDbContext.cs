using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TabibApp.Application.Dtos;

namespace TabibApp.Infrastructure.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
    
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            SeedingData.SeedGovernorates(builder);
            SeedingData.SeedSpecializations(builder);
            SeedingData.SeedRoles(builder);


        }
      

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalHistoryRecord> MedicalHistoryRecords { get; set; }

        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<ChatbotMessage> ChatbotMessages { get; set; }
        public DbSet<Specialization?> Specializations { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<ClinicAddress> ClinicAddresses { get; set; }
        public DbSet<AppointmentSlot> AppointmentSlots { get; set; }


       
    }
   

}
