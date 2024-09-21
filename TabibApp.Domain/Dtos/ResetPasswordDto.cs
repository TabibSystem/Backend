using System.ComponentModel.DataAnnotations;

namespace TabibApp.Application.Dtos;

public class ResetPasswordDto
{
    [Required]
    public string? password { get; set; }
    [Required]
    [Compare("password", ErrorMessage = "password does not match")]
    public string? confirmpassword { get; set; }
    public string? Token { get; set; }
    public string? Email { get; set; }
    
}