using Microsoft.AspNetCore.Mvc;
using Moq;
using TabibApp.Application;
using TabibApp.Application.Dtos;
using TabibApp.Application.Implementation;
using Xunit;

namespace TabibApp.Tests
{
    public class PatientControllerTests
    {
        private readonly Mock<IPatientRepository> _mockRepo;
        private readonly PatientController _controller;

        public PatientControllerTests()
        {
            _mockRepo = new Mock<IPatientRepository>();
            _controller = new PatientController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfPatients()
        {
            // Arrange
            var patients = new List<PatientDto> { new PatientDto { Id = "1", FirstName = "John", LastName = "Doe" } };
            _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(patients);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<PatientDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenPatientExists()
        {
            // Arrange
            var patientId = "1";
            var patient = new PatientDto { Id = patientId, FirstName = "John", LastName = "Doe" };
            _mockRepo.Setup(repo => repo.GetById(patientId)).ReturnsAsync(patient);

            // Act
            var result = await _controller.GetById(patientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PatientDto>(okResult.Value);
            Assert.Equal(patientId, returnValue.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenPatientDoesNotExist()
        {
            // Arrange
            var patientId = "1";
            _mockRepo.Setup(repo => repo.GetById(patientId)).ReturnsAsync((PatientDto)null);

            // Act
            var result = await _controller.GetById(patientId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Add_ReturnsCreatedAtAction_WhenPatientIsValid()
        {
            // Arrange
            var patientDto = new PatientDto { Id = "1", FirstName = "John", LastName = "Doe", Age = 30 };
            _mockRepo.Setup(repo => repo.Add(It.IsAny<Patient>())).ReturnsAsync(new Patient());
            _mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.Add(patientDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<PatientDto>(createdAtActionResult.Value);
            Assert.Equal(patientDto.Id, returnValue.Id);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WhenPatientIsUpdated()
        {
            // Arrange
            var patientDto = new PatientDto { Id = "1", FirstName = "John", LastName = "Doe", Age = 30 };
            _mockRepo.Setup(repo => repo.Update(patientDto)).ReturnsAsync(patientDto);

            // Act
            var result = await _controller.Update(patientDto.Id, patientDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PatientDto>(okResult.Value);
            Assert.Equal(patientDto.Id, returnValue.Id);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenPatientIsDeleted()
        {
            // Arrange
            var patientId = "1";
            _mockRepo.Setup(repo => repo.Delete(patientId)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(patientId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
