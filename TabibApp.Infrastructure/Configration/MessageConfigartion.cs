using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabibApp.Infrastructure.Configration
{
    public class MessageConfigartion: IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {

            builder.Property(m => m.Content).IsRequired().HasMaxLength(1000);
            builder.Property(m => m.SentAt).IsRequired();

            builder.HasOne(m => m.Chat)
                   .WithMany(c => c.Messages)
                   .HasForeignKey(m => m.ChatId);
        }
    }
}
