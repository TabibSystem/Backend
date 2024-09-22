namespace TabibApp.Application.Dtos;

public class Medicine
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public int SessionId { get; set; }
    public Session Session { get; set; }
}