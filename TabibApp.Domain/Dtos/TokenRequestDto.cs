using System.ComponentModel.DataAnnotations;

namespace TabibApp.Application.Dtos;

public class TokenRequestDto
{
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}