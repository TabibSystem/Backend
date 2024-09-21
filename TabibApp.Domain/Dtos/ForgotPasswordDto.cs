using System.ComponentModel.DataAnnotations;

namespace TabibApp.Application.Dtos;

public class ForgotPasswordDto
{
    [Required] 
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string? ClientUrl { get; set; } 
}