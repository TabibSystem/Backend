    using Microsoft.AspNetCore.Identity;
    using TabibApp.Application.Dtos;


    public class ApplicationUser:IdentityUser
        {

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public Doctor Doctor { get; set; }
            public Patient Patient { get; set; }
            public Assistant Assistant { get; set; }

            public ICollection<ChatUser> Chats { get; set; }
            public ICollection<Message> Messages { get; set; }
            public ICollection<Notification> Notifications { get; set; }
            public ICollection<ChatbotMessage> ChatbotMessages { get; set; }
            public List<RefreshToken> RefreshTokens { get; set; }


        }

