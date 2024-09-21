using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace TabibApp.Application.Dtos;

public class RegisterDoctorDto
{
    [Required, MaxLength(50)]
    public string FirstName { get; set; }
    [Required, MaxLength(50)]
    public string LastName { get; set; }
    [Required,MaxLength(15)]
    public string PhoneNumber { get; set; }
    [Required,EmailAddress]
    public string Email { get; set; }
    [Required,PasswordPropertyText]
    public string Password { get; set; }
    [Required,Compare("Password")]
    public string ConfirmPassword { get; set; }
    public string? Bio { get; set; }
    public int Age { get; set; }
    public bool Gender { get; set; }

    public IFormFile ProfilePicture { get; set; }
    public IFormFile WorkCertificate { get; set; }    public int SpecializationId { get; set; }
    public int GovernorateId { get; set; }
    public string? ClientUrl { get; set; }


}