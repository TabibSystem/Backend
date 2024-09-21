using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TabibApp.Application.Dtos;

namespace TabibApp.Application.Implementation
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        /// <summary>
        /// Gets all patients.
        /// </summary>
        /// <returns>A list of patients.</returns>
        /// <response code="200">Returns the list of patients.</response>
        [Authorize("Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PatientDto>), 200)]
        
        public async Task<IActionResult> GetAll()
        {
            var patients = await _patientRepository.GetAll();
            return Ok(patients);
        }

        /// <summary>
        /// Gets a patient by ID.
        /// </summary>
        /// <param name="id">The ID of the patient.</param>
        /// <returns>The patient with the specified ID.</returns>
        /// <response code="200">Returns the patient with the specified ID.</response>
        /// <response code="400">If the patient ID is null or empty.</response>
        /// <response code="404">If the patient is not found.</response>
        [Authorize("Patient")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PatientDto), 200)]

        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "Patient ID cannot be null or empty." });
            }

            var patient = await _patientRepository.GetById(id);
            if (patient == null)
            {
                return NotFound(new { message = "Patient not found." });
            }

            return Ok(patient);
        }

        /// <summary>
        /// Adds a new patient.
        /// </summary>
        /// <param name="patientDto">The patient data transfer object.</param>
        /// <returns>The created patient.</returns>
        /// <response code="201">Returns the newly created patient.</response>
        /// <response code="400">If the patient data is invalid.</response>
        [Authorize("Admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PatientDto patientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = new Patient
            {
                ApplicationUserId = patientDto.Id,
                ApplicationUser = new ApplicationUser
                {
                    Id = patientDto.Id,
                    FirstName = patientDto.FirstName,
                    LastName = patientDto.LastName,
                    PhoneNumber = patientDto.PhoneNumber
                },
                Age = patientDto.Age
            };

            await _patientRepository.Add(patient);
            await _patientRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = patient.ApplicationUserId }, patientDto);
        }

        /// <summary>
        /// Updates an existing patient.
        /// </summary>
        /// <param name="id">The ID of the patient to update.</param>
        /// <param name="patientDto">The updated patient data transfer object.</param>
        /// <returns>The updated patient.</returns>
        /// <response code="200">Returns the updated patient.</response>
        /// <response code="400">If the patient ID is null or empty, or if the patient data is invalid.</response>
        /// <response code="404">If the patient is not found.</response>
        [Authorize("Patient")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PatientDto), 200)]

        public async Task<IActionResult> Update(string id, [FromBody] PatientDto patientDto)
        {
            if (id != patientDto.Id)
            {
                return BadRequest(new { message = "Patient ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedPatient = await _patientRepository.Update(patientDto);
                return Ok(updatedPatient);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a patient by ID.
        /// </summary>
        /// <param name="id">The ID of the patient to delete.</param>
        /// <returns>No content if the deletion was successful.</returns>
        /// <response code="204">If the patient was successfully deleted.</response>
        /// <response code="404">If the patient is not found.</response>
        [HttpDelete("{id}")]
        [Authorize("Admin,Patient")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _patientRepository.Delete(id);
            if (!result)
            {
                return NotFound(new { message = "Patient not found." });
            }

            return NoContent();
        }
    }
}