using TabibApp.Application.Dtos;

namespace TabibApp.Application;

public interface IChatRepository
{
    ChatDto GetChat(int id);
    Task CreateRoom(string name, string userId);
    Task JoinRoom(int chatId, string userId);
    IEnumerable<ChatDto> GetChats(string userId);
    Task<int> CreatePrivateRoom(string rootId, string targetId,int appointmentId);
    IEnumerable<PrivateChatDto> GetPrivateChats(string userId);
    Task<Chat> CreateAppointmentChat(int appointmentId);

    Task<Message> CreateMessage(int chatId, string message, string userId);
    
}