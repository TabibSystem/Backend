using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TabibApp.Api.Hubs;
using TabibApp.Application;

namespace TabibApp.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ChatController : BaseController
{
    private readonly IChatRepository _chatRepository;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatController(IChatRepository chatRepository, IHubContext<ChatHub> hubContext)
    {
        _chatRepository = chatRepository;
        _hubContext = hubContext;
    }

        
        //1
    [HttpGet("private")]
    public IActionResult Private()
    {
        var chats = _chatRepository.GetPrivateChats(GetUserId());
        return Ok(chats);
    }
      //2
    [HttpGet("{id}")]
    public IActionResult Chat(int id)
    {
        var chat = _chatRepository.GetChat(id);
        return Ok(chat); 
    }

       
    [HttpPost]
    public async Task<IActionResult> CreateRoom(string name)
    {
        await _chatRepository.CreateRoom(name, GetUserId());
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> JoinRoom(int id)
    {
        await _chatRepository.JoinRoom(id, GetUserId());

        return RedirectToAction("Chat", "Home", new { id = id });
    }

      

    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage(int roomId, string message,string SenderId)
    {
        var messageEntity = await _chatRepository.CreateMessage(roomId, message, GetUserId());

        await _hubContext.Clients.Group(roomId.ToString())
            .SendAsync("ReceiveMessage", new
            {
                Text = messageEntity.Content,
                Name = messageEntity.Name,
                Timestamp = messageEntity.SentAt.ToString("dd/MM/yyyy hh:mm:ss")
            });

        return Ok(); 
    }
}