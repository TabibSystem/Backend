using System.IdentityModel.Tokens.Jwt;
using System.Transactions;
using Azure.Core;
using EmailService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;
using TabibApp.Infrastructure.Data;

namespace TabibApp.Application.Implementation;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _usermanger;
    private readonly ITokenService _tokenService;
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly AppDbContext _dbContext;
    private readonly IEmailSender _emailsender;
    private readonly IWebHostEnvironment _webHostEnvironment;


    public AuthService(UserManager<ApplicationUser> usermanger
        , ITokenService tokenService,
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository, 
        AppDbContext dbContext, IEmailSender emailsender,IWebHostEnvironment webHostEnvironment)
    {
        this._usermanger = usermanger;
        _tokenService = tokenService;
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _dbContext = dbContext;
        this._emailsender = emailsender;
        _webHostEnvironment = webHostEnvironment;
    }



    public async Task<AuthResultDto> RegisterPatientAsync(RegisterPatientDto registerPatientDto)
    {

        
        if (await _usermanger.FindByEmailAsync(registerPatientDto.Email) is not null)
        {
            return new AuthResultDto { Message = "Email already exists", IsSucceeded = false };
        }

        var user = new ApplicationUser
        {
            Email = registerPatientDto.Email,
            UserName = registerPatientDto.Email,
            FirstName = registerPatientDto.FirstName,
            LastName = registerPatientDto.LastName,
            PhoneNumber = registerPatientDto.PhoneNumber
        };

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {

            var result = await _usermanger.CreateAsync(user, registerPatientDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
                return new AuthResultDto
                {
                    Message = errors,
                    IsSucceeded = false
                };
            }

            var roleResult = await _usermanger.AddToRoleAsync(user, "patient");
            if (!roleResult.Succeeded)
            {
                var roleErrors = string.Join(Environment.NewLine, roleResult.Errors.Select(e => e.Description));
                return new AuthResultDto
                {
                    Message = "Failed to add user to patient role: " + roleErrors,
                    IsSucceeded = false
                };
            }

            var patient = new Patient()
            {
                ApplicationUserId = user.Id,
                Age = registerPatientDto.Age,
                Gender = registerPatientDto.Gender
                
            };

            await _patientRepository.Add(patient);
            await _patientRepository.SaveChangesAsync();

            await transaction.CommitAsync();
            var token = await _usermanger.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>();
            param.Add("token", token);
            param.Add("email", user.Email);
            var callback = QueryHelpers.AddQueryString(registerPatientDto.ClientUrl, param);

            _emailsender.CreateAccountConfirmationEmail(user, callback);
            return new AuthResultDto
            {
                Id=user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = new List<string> { "patient" },
                IsSucceeded = true
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new AuthResultDto
            {
                Message = $"An error occurred: {ex.Message}",
                IsSucceeded = false
            };
        }
    }

    public async Task<AuthResultDto> RegisterDoctorAsync(RegisterDoctorDto registerDto)
    {
        if (await _usermanger.FindByEmailAsync(registerDto.Email) is not null)
        {
            return new AuthResultDto { Message = "Email already exists" };
        }

        var user = new ApplicationUser
        {
            Email = registerDto.Email,
            UserName = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            PhoneNumber = registerDto.PhoneNumber
        };
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(); // Start the transaction

        try
        {
            var result = await _usermanger.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
                return new AuthResultDto
                {
                    Message = errors,
                    IsSucceeded = false
                };

            }

            await _usermanger.AddToRoleAsync(user, "Doctor");
            var profilePicturePath = await SaveFileAsync(registerDto.ProfilePicture);
            var workCertificatePath = await SaveFileAsync(registerDto.WorkCertificate);

            var doctor = new Doctor()
            {

                ApplicationUserId = user.Id,
                Bio = registerDto.Bio,
                SpecializationId = registerDto.SpecializationId,
                GovernorateId = registerDto.GovernorateId,
                ProfilePictureUrl = profilePicturePath,
                WorkCertificateUrl = workCertificatePath,
                Age = registerDto.Age,
                Gender=registerDto.Gender

            };

            await _doctorRepository.Add(doctor);
          
            var token = await _usermanger.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>();
            param.Add("token", token);
            param.Add("email", user.Email);
            var callback = QueryHelpers.AddQueryString(registerDto.ClientUrl, param);

            _emailsender.CreateAccountConfirmationEmail(user, callback);
            await _usermanger.UpdateAsync(user);
            await transaction.CommitAsync();
            return new AuthResultDto()
            {
                Id=user.Id,
                Message = "Registration successful",
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = new List<string> { "Doctor" },
                IsSucceeded = true

            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new AuthResultDto
            {
                Message = $"An error occurred: {ex.Message}",
                IsSucceeded = false
            };
        }




    }


    private async Task<string> SaveFileAsync(IFormFile file)
    { if (file == null || file.Length == 0)
         {
             return null;
         }
     
         try
         {
             var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
             if (!Directory.Exists(uploadsFolder))
             {
                 Directory.CreateDirectory(uploadsFolder);
             }
     
             var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
             var filePath = Path.Combine(uploadsFolder, fileName);
     
             using (var fileStream = new FileStream(filePath, FileMode.Create))
             {
                 await file.CopyToAsync(fileStream);
             }
     
             var publicUrl = $"/uploads/{fileName}";
             return publicUrl;
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             throw;
         }
         
    }

    public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
    {
        var authmodel = new AuthResultDto();
        var user = await _usermanger.FindByEmailAsync(loginDto.Email);
        if (user is null || !await _usermanger.CheckPasswordAsync(user, loginDto.Password))
        {
            authmodel.Message = "Invalid Email/ Password";
            return authmodel;
        }
        if (!await _usermanger.IsEmailConfirmedAsync(user))
        {
            authmodel.Message = "Email not confirmed";
            return authmodel;
        }


        var jwtToken = await _tokenService.GenerateJwtToken(user);
        authmodel.IsSucceeded = true;
        authmodel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        authmodel.FirstName = user.FirstName;
        authmodel.LastName = user.LastName;
        authmodel.Email = user.Email;
        authmodel.Roles = await _usermanger.GetRolesAsync(user) as List<string>;
        authmodel.Id = user.Id;


        return authmodel;
    }

    public async Task<AuthResultDto> ConfirmEmailAsync(string email, string token)
    {
        var user = await _usermanger.FindByEmailAsync(email);
        if (user == null)
        {
            return new AuthResultDto
            {
                Message = "User not found",
                IsSucceeded = false
            };
        }

        var result = await _usermanger.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
        {
            var errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
            return new AuthResultDto
            {
                Message = $"Email confirmation failed: {errors}",
                IsSucceeded = false
            };
        }


        return new AuthResultDto
        {
            Message = "Email confirmed successfully",
            IsSucceeded = true,

        };
    }


    public async Task<IActionResult> SendEmailconfirmation(string email, string ClientUrl)
    {
        var user = await _usermanger.FindByEmailAsync(email);
        if (user is  null)
         return new BadRequestObjectResult(new { message = "Email does not exist" });
        
        
        var token = await _usermanger.GenerateEmailConfirmationTokenAsync(user);
        var param = new Dictionary<string, string?>();
        param.Add("token", token);
        param.Add("email", user.Email);
        var callback = QueryHelpers.AddQueryString(ClientUrl, param);

        _emailsender.CreateAccountConfirmationEmail(user, callback);
        await _usermanger.UpdateAsync(user);
        return new OkObjectResult(new { message = "Confirmation email sent" });
        




    }

 

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPassword)
    {
        var user = await _usermanger.FindByEmailAsync(forgotPassword.Email);
        if(user is null)
            return false;
        var token = await _usermanger.GeneratePasswordResetTokenAsync(user);
        var param = new Dictionary<string, string?>();
        param.Add("token",token);
        param.Add("email",forgotPassword.Email);
        var callback = QueryHelpers.AddQueryString(forgotPassword.ClientUrl, param);
        _emailsender.CreatePasswordConfirmationEmail(user,callback);
        await _usermanger.UpdateAsync(user);

        return true;

    }

    public async Task<ResetPasswordResultDto> ResetPasswordAsync(ResetPasswordDto resetPassword)
    {
        
        var user = await _usermanger.FindByEmailAsync(resetPassword.Email);
        if (user == null)
        {
            return new ResetPasswordResultDto
            {
                Message = "Invalid email address",
                IsSucceeded = false
            };
        }

        var resetResult = await _usermanger.ResetPasswordAsync(user, resetPassword.Token!, resetPassword.password!);
        if (resetResult.Succeeded)
        {
            return new ResetPasswordResultDto
            {
                Message = "Password has been reset successfully.",
                IsSucceeded = true
            };
        }

        var errors = string.Join(", ", resetResult.Errors.Select(e => e.Description));
        return new ResetPasswordResultDto
        {
            Message = $"Failed to reset password: {errors}",
            IsSucceeded = false
        };
    }
    
}




    

   
