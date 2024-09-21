using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace TabibApp.Infrastructure.Configration
{
    public class NotificationConfigration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {

            builder.Property(n => n.Message).IsRequired();
            builder.Property(n => n.CreatedAt).IsRequired();
            builder.Property(n => n.IsRead).IsRequired();

            builder.HasOne(n => n.User)
                   .WithMany(u => u.Notifications)
                   .HasForeignKey(n => n.UserId);
        }
    }
}
