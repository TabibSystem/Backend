using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TabibApp.Api.Controllers;
using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;
using Xunit;

namespace TabibApp.Tests.Controllers
{
    public class MedicalHistoryControllerTests
    {
        private readonly MedicalHistoryController _controller;
        private readonly Mock<IMedicalHistoryService> _mockMedicalHistoryService;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;

        public MedicalHistoryControllerTests()
        {
            _mockMedicalHistoryService = new Mock<IMedicalHistoryService>();
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _mockWebHostEnvironment.Setup(env => env.WebRootPath).Returns("wwwroot");

            _controller = new MedicalHistoryController(_mockMedicalHistoryService.Object, _mockWebHostEnvironment.Object);
        }
        public async Task UploadMedicalHistory_ReturnsOkResult_WithFileUrl()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var dto = new MedicalHistoryUploadDto { File = fileMock.Object, PatientId = "123" };

            _mockMedicalHistoryService.Setup(s => s.GetpatientId(dto.PatientId)).ReturnsAsync(1);
            _mockMedicalHistoryService.Setup(s => s.Add(It.IsAny<MedicalHistoryRecord>())).ReturnsAsync(true);

            // Act
            var result = await _controller.UploadMedicalHistory(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Dictionary<string, string>>(okResult.Value);
            Assert.Equal("File uploaded successfully", returnValue["message"]);
            Assert.Contains("MedicalHistory", returnValue["fileUrl"]);
        }

        [Fact]
        public async Task GetMedicalHistoryRecords_ReturnsOkResult_WithRecords()
        {
            // Arrange
            var patientId = "123";
            var records = new List<MedicalHistoryResponseDto>
            {
                new MedicalHistoryResponseDto { Id = 1, FileName = "test1.txt", FileUrl = "/MedicalHistory/test1.txt", UploadDate = System.DateTime.Now },
                new MedicalHistoryResponseDto { Id = 2, FileName = "test2.txt", FileUrl = "/MedicalHistory/test2.txt", UploadDate = System.DateTime.Now }
            };

            _mockMedicalHistoryService.Setup(s => s.Get(patientId)).ReturnsAsync(records);

            // Act
            var result = await _controller.GetMedicalHistoryRecords(patientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<MedicalHistoryResponseDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }
    }
}
