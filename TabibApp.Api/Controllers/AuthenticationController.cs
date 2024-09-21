using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;

namespace TabibApp.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController : Controller
{
    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState); 
        var result = await _authService.LoginAsync(loginDto);

        if (!result.IsSucceeded)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }
    

    [HttpPost("RegisterPatient")]
    public async Task<IActionResult> RegisterPatient([FromForm] RegisterPatientDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterPatientAsync(model);
        
        if (!result.IsSucceeded)
        {
            return BadRequest(result.Message);
        }
       
        return Ok(result);
    }

    [HttpPost("RegisterDoctor")]
    public async Task<IActionResult> RegisterDoctor([FromForm] RegisterDoctorDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
    
        var result = await _authService.RegisterDoctorAsync(model);
    
        if (!result.IsSucceeded)
        {
            return BadRequest(result.Message);
        }
        
        return Ok(result);
    }

    [HttpPost]
    [Route("ConfirmEmail")]
    public async Task<IActionResult> Emailconfirmation([FromQuery] string email, [FromQuery] string token)
    {
        var result = await _authService.ConfirmEmailAsync(email, token);
        if(!result.IsSucceeded)
            return BadRequest(result.Message);
        
        return Ok(result);
        
    }


    [HttpPost]
    [Route("SendEmailconfirmation")]
    public async Task<IActionResult> SendEmailconfirmation(string email,string ClientUrl)
    {
        return await _authService.SendEmailconfirmation(email, ClientUrl);
    }

    [HttpPost]
    [Route("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPassword)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _authService.ForgotPasswordAsync(forgotPassword);

        return result ? Ok() : BadRequest();


    }

    [HttpPost]
    [Route("ResetPassword")]
    public async Task<IActionResult> ForgotPassword(ResetPasswordDto resetPassword)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.ResetPasswordAsync(resetPassword);
        if (!result.IsSucceeded)
            return BadRequest(result.Message);

        return Ok(result.Message);
        
    }
    


}