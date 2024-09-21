using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using TabibApp.Application.Dtos;

namespace TabibApp.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto> RegisterPatientAsync(RegisterPatientDto registerPatientDto);
    Task<AuthResultDto> RegisterDoctorAsync(RegisterDoctorDto registerDto);
    Task<AuthResultDto> LoginAsync(LoginDto loginDto);
      Task<AuthResultDto> ConfirmEmailAsync(string email ,string token);
      Task<IActionResult> SendEmailconfirmation(string email, string ClientUrl);

    Task<bool> ForgotPasswordAsync(ForgotPasswordDto ForgotPassword);
    Task<ResetPasswordResultDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}