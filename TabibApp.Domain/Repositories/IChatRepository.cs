﻿namespace TabibApp.Application;

public interface IChatRepository
{
    Chat GetChat(int id);
    Task CreateRoom(string name, string userId);
    Task JoinRoom(int chatId, string userId);
    IEnumerable<Chat> GetChats(string userId);
    Task<int> CreatePrivateRoom(string rootId, string targetId);
    IEnumerable<Chat> GetPrivateChats(string userId);
    Task<Chat> CreateAppointmentChat(int appointmentId);

    Task<Message> CreateMessage(int chatId, string message, string userId);
    
}