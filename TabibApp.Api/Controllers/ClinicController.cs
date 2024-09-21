using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;

namespace TabibApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicService _clinicService;

        public ClinicController(IClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        /// <summary>
        /// Creates a new clinic.
        /// </summary>
        /// <param name="clinicDto">The clinic data transfer object.</param>
        /// <returns>The created clinic.</returns>
        /// <response code="201">Returns the newly created clinic.</response>
        /// <response code="400">If the clinic data is invalid or the clinic already exists.</response>
        /// <response code="404">If the clinic could not be created.</response>
        [Authorize("Doctor")]
        [HttpPost]
        [ProducesResponseType(typeof(ClinicDto), 201)]
 
        public async Task<IActionResult> CreateClinic([FromBody] ClinicDto clinicDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _clinicService.ClinicExistsAsync(clinicDto.Name, clinicDto.PhoneNumber))
                return BadRequest(new { Message = "Clinic already exists" });

            var createdClinic = await _clinicService.CreateClinicAsync(clinicDto);
            if (createdClinic == null)
            {
                return NotFound(new { Message = "Clinic could not be created" });
            }

            return StatusCode(201, createdClinic);
        }

        /// <summary>
        /// Gets a clinic by doctor ID.
        /// </summary>
        /// <param name="doctorId">The ID of the doctor.</param>
        /// <returns>The clinic associated with the specified doctor ID.</returns>
        /// <response code="200">Returns the clinic associated with the specified doctor ID.</response>
        /// <response code="404">If the clinic is not found.</response>
        [Authorize("Doctor")]
        [HttpGet("doctor/{doctorId}")]
        [ProducesResponseType(typeof(ClinicDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetClinicById(string doctorId)
        {
            var clinic = await _clinicService.GetClinicById(doctorId);
            if (clinic == null)
                return NotFound(new { Message = "Clinic not found" });

            return Ok(clinic);
        }

        /// <summary>
        /// Updates an existing clinic.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic to update.</param>
        /// <param name="clinicDto">The updated clinic data transfer object.</param>
        /// <returns>The updated clinic.</returns>
        /// <response code="200">Returns the updated clinic.</response>
        /// <response code="400">If the clinic data is invalid.</response>
        /// <response code="404">If the clinic is not found.</response>
        [Authorize("Doctor")]
        [HttpPut("{clinicId}")]
        [ProducesResponseType(typeof(ClinicDto), 200)]
       
        public async Task<IActionResult> UpdateClinic(int clinicId, [FromBody] ClinicDto clinicDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedClinic = await _clinicService.UpdateClinicAsync(clinicId, clinicDto);
            if (updatedClinic == null)
                return NotFound(new { Message = "Clinic not found" });

            return Ok(updatedClinic);
        }

        /// <summary>
        /// Deletes a clinic by ID.
        /// </summary>
        /// <param name="clinicId">The ID of the clinic to delete.</param>
        /// <returns>No content if the deletion was successful.</returns>
        /// <response code="204">If the clinic was successfully deleted.</response>
        /// <response code="404">If the clinic is not found.</response>
        [Authorize(Policy = "Doctor")]
        [HttpDelete("{clinicId}")]
        public async Task<IActionResult> DeleteClinic(int clinicId)
        {
            var isDeleted = await _clinicService.DeleteClinicAsync(clinicId);

            if (!isDeleted)
                return NotFound(new { Message = "Clinic not found" });

            return NoContent();
        }
    }
}