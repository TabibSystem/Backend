using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using TabibApp.Api.Controllers;
using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;
using Xunit;

namespace TabibApp.Tests.Controllers
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthenticationController _controller;

        public AuthenticationControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthenticationController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WithAuthResult()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "User@example.com", Password = "Password123" };
            var authResult = new AuthResultDto { IsSucceeded = true, Message = "Login successful" };
            _mockAuthService.Setup(s => s.LoginAsync(loginDto)).ReturnsAsync(authResult);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(authResult, okResult.Value);
        }

        [Fact]
        public async Task RegisterPatient_ReturnsOkResult_WithAuthResult()
        {
            // Arrange
            var registerPatientDto = new RegisterPatientDto
            {
                FirstName = "Ahmed",
                LastName = "Zaki",
                Email = "Ahmed.Zaki@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123",
                PhoneNumber = "1234567890",
                Age = 30,
                Gender = true
            };
            var authResult = new AuthResultDto { IsSucceeded = true, Message = "Registration successful" };
            _mockAuthService.Setup(s => s.RegisterPatientAsync(registerPatientDto)).ReturnsAsync(authResult);

            // Act
            var result = await _controller.RegisterPatient(registerPatientDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(authResult, okResult.Value);
        }

        [Fact]
        public async Task RegisterDoctor_ReturnsOkResult_WithAuthResult()
        {
            // Arrange
            var registerDoctorDto = new RegisterDoctorDto
            {
                FirstName = "Ahmed",
                LastName = "Zaki",
                Email = "Ahmed.Zaki@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123",
                PhoneNumber = "0987654321",
                Age = 40,
                Gender = false,
                SpecializationId = 1,
                GovernorateId = 1
            };
            var authResult = new AuthResultDto { IsSucceeded = true, Message = "Registration successful" };
            _mockAuthService.Setup(s => s.RegisterDoctorAsync(registerDoctorDto)).ReturnsAsync(authResult);

            // Act
            var result = await _controller.RegisterDoctor(registerDoctorDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(authResult, okResult.Value);
        }

        [Fact]
        public async Task Emailconfirmation_ReturnsOkResult_WithAuthResult()
        {
            // Arrange
            var email = "test@example.com";
            var token = "token123";
            var authResult = new AuthResultDto { IsSucceeded = true, Message = "Email confirmed" };
            _mockAuthService.Setup(s => s.ConfirmEmailAsync(email, token)).ReturnsAsync(authResult);

            // Act
            var result = await _controller.Emailconfirmation(email, token);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(authResult, okResult.Value);
        }

        [Fact]
        public async Task SendEmailconfirmation_ReturnsOkResult()
        {
            // Arrange
            var email = "User@example.com";
            var clientUrl = "http://Tabib.com";
            var actionResult = new OkResult();
            _mockAuthService.Setup(s => s.SendEmailconfirmation(email, clientUrl)).ReturnsAsync(actionResult);

            // Act
            var result = await _controller.SendEmailconfirmation(email, clientUrl);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ForgotPassword_ReturnsOkResult()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto { Email = "User@example.com", ClientUrl = "http://example.com" };
            _mockAuthService.Setup(s => s.ForgotPasswordAsync(forgotPasswordDto)).ReturnsAsync(true);

            // Act
            var result = await _controller.ForgotPassword(forgotPasswordDto);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ResetPassword_ReturnsOkResult_WithMessage()
        {
            // Arrange
            var resetPasswordDto = new ResetPasswordDto
            {
                password = "NewPassword123",
                confirmpassword = "NewPassword123",
                Token = "tokenehweiughweugwueghweogew98u9823g23g2wgwe",
                Email = "User@example.com"
            };
            var resetPasswordResult = new ResetPasswordResultDto { IsSucceeded = true, Message = "Password reset successful" };
            _mockAuthService.Setup(s => s.ResetPasswordAsync(resetPasswordDto)).ReturnsAsync(resetPasswordResult);

            // Act
            var result = await _controller.ForgotPassword(resetPasswordDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(resetPasswordResult.Message, okResult.Value);
        }
    }
}
