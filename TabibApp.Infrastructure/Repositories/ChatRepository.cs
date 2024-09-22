using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TabibApp.Application;
using TabibApp.Application.Dtos;
using TabibApp.Infrastructure.Data;

namespace TabibApp.Infrastructure.Repository;

public class ChatRepository : IChatRepository
{
    public AppDbContext _ctx;
    private readonly ILogger<ChatRepository> _logger;


    public ChatRepository(AppDbContext ctx,ILogger<ChatRepository> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

        public async Task<Chat> CreateAppointmentChat(int appointmentId)
        {
            var appointment = await _ctx.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
                throw new Exception("Appointment not found");

            var patinetname= appointment.Patient.ApplicationUser.FirstName+" "+appointment.Patient.ApplicationUser.LastName  ;
            var Doctorname= appointment.Doctor.ApplicationUser.FirstName+" "+appointment.Doctor.ApplicationUser.LastName  ;
            var chat = new Chat
            {
                Name = $"Chat between {patinetname} and Dr. {Doctorname}",
                Type = ChatType.Private,
                AppointmentId = appointmentId
            };

            // Add both patient and doctor to the chat
            chat.Users.Add(new ChatUser
            {
                UserId = appointment.Patient.ApplicationUserId,
                Role = UserRole.Member
            });

            chat.Users.Add(new ChatUser
            {
                UserId = appointment.Doctor.ApplicationUserId,
                Role = UserRole.Member
            });

            _ctx.Chats.Add(chat);
            await _ctx.SaveChangesAsync();

            appointment.ChatId = chat.Id;
            await _ctx.SaveChangesAsync();

            return chat;
        }


        
        public async Task<Message> CreateMessage(int chatId, string content, string receiverId)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentException("Message content cannot be null or empty.", nameof(content));
            }

            if (string.IsNullOrEmpty(receiverId))
            {
                throw new ArgumentException("Receiver ID cannot be null or empty.", nameof(receiverId));
            }

        

            // Check if the ChatId exists in the Chats table
            var chatExists = await _ctx.Chats.AnyAsync(c => c.Id == chatId);
            if (!chatExists)
            {
                throw new ArgumentException("Chat ID does not exist.", nameof(chatId));
            }

            var message = new Message
            {
                ChatId = chatId,
                Content = content,
                ReceiverId = receiverId,
                SentAt = DateTime.UtcNow,
                Name = receiverId // Ensure Name is set
            };

            try
            {
                _ctx.Messages.Add(message);
                await _ctx.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the message.");
                throw;
            }

            return message;
        }
        public async Task<int> CreatePrivateRoom(string rootId, string targetId,int appointmentId)
        {
            if (string.IsNullOrEmpty(rootId))
            {
                throw new ArgumentNullException(nameof(rootId), "Root ID cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(targetId))
            {
                throw new ArgumentNullException(nameof(targetId), "Target ID cannot be null or empty.");
            }

            var chat = new Chat
            {
                Type = ChatType.Private,
                Users = new List<ChatUser>() ,
                Name="chat",
                AppointmentId =  appointmentId
            };

            chat.Users.Add(new ChatUser
            {
                UserId = targetId
            });

            chat.Users.Add(new ChatUser
            {
                UserId = rootId
            });

            _ctx.Chats.Add(chat);

            await _ctx.SaveChangesAsync();

            return chat.Id;
        }
        public async Task CreateRoom(string name, string userId)
        {
            var chat = new Chat
            {
                Name = name,
                Type = ChatType.Room
            };

            chat.Users.Add(new ChatUser
            {
                UserId = userId,
                Role = UserRole.Admin
            });

            _ctx.Chats.Add(chat);

            await _ctx.SaveChangesAsync();
        }

        public ChatDto GetChat(int id)
        {
            var chat = _ctx.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);

            if (chat == null)
            {
                return null;
            }

            var chatDto = new ChatDto
            {
                Id = chat.Id,
                Name = chat.Name,
                Messages = chat.Messages.Select(m => new MessageDto
                {
                    Name = m.Name,
                    Text = m.Content,
                    Timestamp = m.SentAt.ToString("dd/MM/yyyy hh:mm:ss")
                }).ToList()
            };

            return chatDto;
        }

        //done//public
        public IEnumerable<ChatDto> GetChats(string userId)
        {
            return _ctx.Chats
                .Include(x => x.Users)
                .Where(x => !x.Users.Any(y => y.UserId == userId))
                .Select(chat => new ChatDto
                {
                    Id = chat.Id,
                    Name = chat.Name
                })
                .ToList();
        }
        public IEnumerable<PrivateChatDto> GetPrivateChats(string userId)
        {
            return _ctx.Chats
                .Include(x => x.Users)
                .ThenInclude(x => x.User)
                .Where(x => x.Type == ChatType.Private
                            && x.Users.Any(y => y.UserId == userId))
                .Select(chat => new PrivateChatDto
                {
                    Id = chat.Id,
                    UserName = chat.Users.FirstOrDefault(x => x.UserId != userId).User.UserName
                })
                .ToList();
        }

        //done
        public async Task JoinRoom(int chatId, string userId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId,
                Role = UserRole.Member
            };

            _ctx.ChatUsers.Add(chatUser);

            await _ctx.SaveChangesAsync();
        }
}