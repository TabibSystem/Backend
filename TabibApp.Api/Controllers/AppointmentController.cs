using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TabibApp.Application;
using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;

namespace TabibApp.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentController(IAppointmentService appointmentService, IAppointmentRepository appointmentRepository)
        {
            _appointmentService = appointmentService;
            _appointmentRepository = appointmentRepository;
        }


        /// <summary>
        /// Generates appointment slots for a clinic.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic.</param>
        /// <param name="daySchedules">The day schedules for the clinic.</param>
        /// <param name="duration">The duration of each appointment slot.</param>
        /// <param name="DoctorId">The ID of the doctor.</param>
        /// <returns>A list of generated appointment slots.</returns>
        /// <response code="200">Returns the list of generated appointment slots.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [Authorize("Doctor")]
        [HttpPost("/GenerateAppointmentSlots")]
        public async Task<IActionResult> Createslots(int clinicId, List<DayScheduleDto> daySchedules, TimeSpan duration, string DoctorId)
        {
            try
            {
                var slots = await _appointmentService.GenerateAppointmentSlots(clinicId, daySchedules, duration, DoctorId);
                return Ok(slots);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets appointment slots for a clinic by clinic ID.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic.</param>
        /// <returns>A list of appointment slots for the clinic.</returns>
        /// <response code="200">Returns the list of appointment slots for the clinic.</response>
        /// <response code="404">If no appointment slots are found for the given clinic ID.</response>
        [Authorize("Doctor")]
        [HttpGet("/clinic/{clinicId}/appointment-slots")]
        public async Task<IActionResult> GetAppointmentSlotsByClinicId(int clinicId)
        {
            var slots = await _appointmentRepository.GetAppointmentSlotsByClinicId(clinicId);
            if (slots == null || !slots.Any())
            {
                return NotFound(new { message = "No appointment slots found for the given clinic ID." });
            }

            return Ok(slots);
        }

        /// <summary>
        /// Books an appointment.
        /// </summary>
        /// <param name="appointmentDto">The appointment data transfer object.</param>
        /// <returns>The booked appointment.</returns>
        /// <response code="201">Returns the booked appointment.</response>
        /// <response code="400">If the booking data is invalid or the booking fails.</response>
        [Authorize("Doctor,Patient")]
        [HttpPost("/book")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentDto appointmentDto)
        {
            try
            {
                var result = await _appointmentRepository.BookAppointment(appointmentDto);
                return CreatedAtAction(nameof(BookAppointment), result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets appointments for a patient by patient ID.
        /// </summary>
        /// <param name="PatientId">The ID of the patient.</param>
        /// <returns>A list of appointments for the patient.</returns>
        /// <response code="200">Returns the list of appointments for the patient.</response>
        /// <response code="404">If no appointments are found for the given patient ID.</response>
        [Authorize("Doctor,Patient")]
        [HttpGet("/patient/{PatientId}")]
        public async Task<IActionResult> GetAppointmentsForPatient(string PatientId)
        {
            var appointments = await _appointmentRepository.GetAppointmentsForPatient(PatientId);
            if (appointments == null || !appointments.Any())
            {
                return NotFound(new { message = "No appointments found for the given patient ID." });
            }

            return Ok(appointments);
        }

        /// <summary>
        /// Gets appointments for a doctor by doctor ID.
        /// </summary>
        /// <param name="DoctorId">The ID of the doctor.</param>
        /// <returns>A list of appointments for the doctor.</returns>
        /// <response code="200">Returns the list of appointments for the doctor.</response>
        /// <response code="404">If no appointments are found for the given doctor ID.</response>
        [Authorize("Doctor")]
        [HttpGet("/doctor/{DoctorId}")]
        [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), 200)]
        public async Task<IActionResult> GetAppointmentsForDoctor(string DoctorId)
        {
            var appointments = await _appointmentRepository.GetAppointmentsForDoctor(DoctorId);
            if (appointments == null || !appointments.Any())
            {
                return NotFound(new { message = "No appointments found for the given doctor ID." });
            }

            return Ok(appointments);
        }

        /// <summary>
        /// Cancels an appointment by appointment slot ID.
        /// </summary>
        /// <param name="AppointmentSlotId">The ID of the appointment slot to cancel.</param>
        /// <returns>No content if the cancellation was successful.</returns>
        /// <response code="200">If the appointment was successfully canceled.</response>
        /// <response code="404">If the appointment slot is not found.</response>
        /// <response code="400">If the cancellation fails.</response>
        [Authorize("Doctor")]
        [HttpDelete("/cancel/{AppointmentSlotId}")]
     
        public async Task<IActionResult> CancelAppointment(int AppointmentSlotId)
        {
            try
            {
                if (await _appointmentRepository.CancelAppointment(AppointmentSlotId))
                    return Ok();

                return NotFound(new { message = "Appointment slot not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}