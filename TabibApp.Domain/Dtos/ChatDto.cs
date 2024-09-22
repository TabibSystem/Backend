namespace TabibApp.Application.Dtos;

public class ChatDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<MessageDto> Messages { get; set; }

}