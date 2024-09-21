using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TabibApp.Application.Dtos;

public class RegisterPatientDto
{
 
    [Required, MaxLength(50)]
    public string FirstName { get; set; }
    [Required, MaxLength(50)]
    public string LastName { get; set; }
    [Required,MaxLength(15)]
    public string PhoneNumber { get; set; }
    [Required]
    public int Age { get; set; }
    public bool Gender { get; set; }

    [Required,EmailAddress]
    public string Email { get; set; }
    [Required,PasswordPropertyText]
    public string Password { get; set; }
    [Required,Compare("Password")]
    public string ConfirmPassword { get; set; }
    public string? ClientUrl { get; set; }
}