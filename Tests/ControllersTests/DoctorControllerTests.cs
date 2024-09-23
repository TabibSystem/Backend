using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabibApp.Api.Controllers;
using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;
using Xunit;

namespace TabibApp.Tests.Controllers
{
    public class DoctorControllerTests
    {
        private readonly Mock<IDoctorService> _mockDoctorService;
        private readonly Mock<ISpecializationService> _mockSpecializationService;
        private readonly DoctorController _controller;

        public DoctorControllerTests()
        {
            _mockDoctorService = new Mock<IDoctorService>();
            _mockSpecializationService = new Mock<ISpecializationService>();
            _controller = new DoctorController(_mockDoctorService.Object, _mockSpecializationService.Object);
        }

        [Fact]
        public async Task GetAllDoctors_ReturnsOkResult_WithListOfDoctors()
        {
            // Arrange
            var doctors = new List<DoctorDto> { new DoctorDto { Id = "1", FirstName = "Saif", LastName = "Swillam" } };
            _mockDoctorService.Setup(service => service.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(doctors);

            // Act
            var result = await _controller.GetAllDoctors(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<DoctorDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetDoctorById_ReturnsNotFound_WhenDoctorDoesNotExist()
        {
            // Arrange
            _mockDoctorService.Setup(service => service.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((DoctorDto)null);

            // Act
            var result = await _controller.GetDoctorById("1");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddDoctor_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Model state is invalid");

            // Act
            var result = await _controller.AddDoctor(new DoctorDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateDoctor_ReturnsBadRequest_WhenDoctorIdMismatch()
        {
            // Arrange
            var doctorDto = new DoctorDto { Id = "1" };

            // Act
            var result = await _controller.UpdateDoctor("2", doctorDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteDoctor_ReturnsNoContent_WhenDoctorIsDeleted()
        {
            // Arrange
            _mockDoctorService.Setup(service => service.DeleteAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteDoctor("1");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
