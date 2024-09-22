
using System.Text.Json.Serialization;
using TabibApp.Application.Dtos;

public class Chat
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ChatType Type { get; set; }
    public ICollection<Message> Messages { get; set; }
    public ICollection<ChatUser> Users { get; set; }
    public int? AppointmentId { get; set; }
    public Appointment Appointment { get; set; }
    
}
