using Microsoft.EntityFrameworkCore;
using TabibApp.Application;
using TabibApp.Application.Dtos;
using TabibApp.Infrastructure.Data;

namespace TabibApp.Infrastructure.Repository;

public class ChatRepository:IChatRepository
{
       private AppDbContext _ctx;

        public ChatRepository(AppDbContext ctx) => _ctx = ctx;

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
                Role = UserRole.Admin
            });

            _ctx.Chats.Add(chat);
            await _ctx.SaveChangesAsync();

            // Link chat to appointment
            appointment.ChatId = chat.Id;
            await _ctx.SaveChangesAsync();

            return chat;
        }

        public async Task<Message> CreateMessage(int chatId, string message, string userId)
        {
            var Message = new Message
            {
                ChatId = chatId,
                Content = message,
                Name = userId,
                SentAt = DateTime.Now
            };

            _ctx.Messages.Add(Message);
            await _ctx.SaveChangesAsync();

            return Message;
        }

        public async Task<int> CreatePrivateRoom(string rootId, string targetId)
        {
            var chat = new Chat
            {
                Type = ChatType.Private
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

        public Chat GetChat(int id)
        {
            return _ctx.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Chat> GetChats(string userId)
        {
            return _ctx.Chats
                .Include(x => x.Users)
                .Where(x => !x.Users
                    .Any(y => y.UserId == userId))
                .ToList();
        }

        public IEnumerable<Chat> GetPrivateChats(string userId)
        {
            return _ctx.Chats
                   .Include(x => x.Users)
                       .ThenInclude(x => x.User)
                   .Where(x => x.Type == ChatType.Private
                       && x.Users
                           .Any(y => y.UserId == userId))
                   .ToList();
        }

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