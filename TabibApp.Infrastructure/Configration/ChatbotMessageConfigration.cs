using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TabibApp.Infrastructure.Configration
{
    public class ChatbotMessageConfigration : IEntityTypeConfiguration<ChatbotMessage>
    {
        public void Configure(EntityTypeBuilder<ChatbotMessage> builder)
        {
        }
    }
}
