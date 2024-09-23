using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabibApp.Api.Controllers;
using TabibApp.Application;
using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;
using Xunit;

namespace TabibApp.Tests.Controllers
{
    public class AppointmentControllerTests
    {
        private readonly Mock<IAppointmentService> _mockAppointmentService;
        private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
        private readonly Mock<ILogger<AppointmentController>> _mockLogger;
        private readonly AppointmentController _controller;

        public AppointmentControllerTests()
        {
            _mockAppointmentService = new Mock<IAppointmentService>();
            _mockAppointmentRepository = new Mock<IAppointmentRepository>();
            _mockLogger = new Mock<ILogger<AppointmentController>>();
            _controller = new AppointmentController(_mockAppointmentService.Object, _mockAppointmentRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Createslots_ReturnsOkResult_WithGeneratedSlots()
        {
            // Arrange
            int clinicId = 1;
            var daySchedules = new List<DayScheduleDto>();
            var duration = TimeSpan.FromMinutes(30);
            string doctorId = "doctor123";
            var slots = new List<AppointmentSlotDto> { new AppointmentSlotDto() };
            _mockAppointmentService.Setup(s => s.GenerateAppointmentSlots(clinicId, daySchedules, duration, doctorId))
                .ReturnsAsync(slots);

            // Act
            var result = await _controller.Createslots(clinicId, daySchedules, duration, doctorId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(slots, okResult.Value);
        }

        [Fact]
        public async Task GetAppointmentSlotsByClinicId_ReturnsOkResult_WithSlots()
        {
            // Arrange
            int clinicId = 1;
            var slots = new List<AppointmentSlotDto> { new AppointmentSlotDto() };
            _mockAppointmentRepository.Setup(r => r.GetAppointmentSlotsByClinicId(clinicId))
                .ReturnsAsync(slots);

            // Act
            var result = await _controller.GetAppointmentSlotsByClinicId(clinicId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(slots, okResult.Value);
        }

        [Fact]
        public async Task BookAppointment_ReturnsCreatedAtActionResult_WithBookedAppointment()
        {
            // Arrange
            var appointmentDto = new BookAppointmentDto();
            var bookedAppointment = new AppointmentDto();
            _mockAppointmentRepository.Setup(r => r.BookAppointment(appointmentDto))
                .ReturnsAsync(bookedAppointment);

            // Act
            var result = await _controller.BookAppointment(appointmentDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(bookedAppointment, createdAtActionResult.Value);
        }

        [Fact]
        public async Task GetAppointmentsForPatient_ReturnsOkResult_WithAppointments()
        {
            // Arrange
            string patientId = "patient123";
            var appointments = new List<AppointmentDto> { new AppointmentDto() };
            _mockAppointmentRepository.Setup(r => r.GetAppointmentsForPatient(patientId))
                .ReturnsAsync(appointments);

            // Act
            var result = await _controller.GetAppointmentsForPatient(patientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(appointments, okResult.Value);
        }

        [Fact]
        public async Task GetAppointmentsForDoctor_ReturnsOkResult_WithAppointments()
        {
            // Arrange
            string doctorId = "doctor123";
            var appointments = new List<AppointmentDto> { new AppointmentDto() };
            _mockAppointmentRepository.Setup(r => r.GetAppointmentsForDoctor(doctorId))
                .ReturnsAsync(appointments);

            // Act
            var result = await _controller.GetAppointmentsForDoctor(doctorId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(appointments, okResult.Value);
        }

        [Fact]
        public async Task CancelAppointment_ReturnsOkResult_WhenCancellationIsSuccessful()
        {
            // Arrange
            int appointmentSlotId = 1;
            _mockAppointmentRepository.Setup(r => r.CancelAppointment(appointmentSlotId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.CancelAppointment(appointmentSlotId);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
