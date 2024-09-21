using System.Text.Json.Serialization;
namespace TabibApp.Application.Dtos;

public class AuthResultDto
{
    public string Id { get; set; }
   public string FirstName { get; set; }
   public  string LastName { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public bool IsSucceeded { get; set; }
    public string Token { get; set; }
    public List<string> Roles { get; set; }
    

  
}