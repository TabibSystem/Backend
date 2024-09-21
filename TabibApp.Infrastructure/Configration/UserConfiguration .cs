using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TabibApp.Infrastructure.Configration
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(u => u.Doctor)
                .WithOne(d => d.ApplicationUser)
                .HasForeignKey<Doctor>(d => d.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Patient)
                .WithOne(p => p.ApplicationUser)
                .HasForeignKey<Patient>(p => p.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Assistant)
                .WithOne(a => a.ApplicationUser)
                .HasForeignKey<Assistant>(a => a.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Chats)
                .WithOne(cu => cu.User)
                .HasForeignKey(cu => cu.UserId);

            builder.HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId);

            builder.HasMany(u => u.ChatbotMessages)
                .WithOne(cb => cb.ApplicationUser)
                .HasForeignKey(cb => cb.ApplicationUserId);
        }
    }
}
