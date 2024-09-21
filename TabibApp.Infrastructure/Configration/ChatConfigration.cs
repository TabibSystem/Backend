using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TabibApp.Infrastructure.Configration
{
    public class ChatConfigration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(c => c.Messages)
                .WithOne(m => m.Chat)
                .HasForeignKey(m => m.ChatId);

            builder.HasMany(c => c.Users)
                .WithOne(cu => cu.Chat)
                .HasForeignKey(cu => cu.ChatId);
            builder.HasOne(c => c.Appointment)
                .WithOne(a => a.Chat)
                .HasForeignKey<Chat>(c => c.AppointmentId);
        }
    }
    
}
