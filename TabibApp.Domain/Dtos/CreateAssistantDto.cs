namespace TabibApp.Application.Dtos;

public class CreateAssistantDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public string DoctorId { get; set; }
}