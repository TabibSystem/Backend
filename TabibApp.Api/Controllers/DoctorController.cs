using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;

namespace TabibApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly ISpecializationService _specializationService;

        public DoctorController(IDoctorService doctorService, ISpecializationService specializationService)
        {
            _doctorService = doctorService;
            _specializationService = specializationService;
        }

        /// <summary>
        /// Gets all doctors with pagination.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A list of doctors.</returns>
        /// <response code="200">Returns the list of doctors.</response>
        /// <response code="400">If the page number is less than or equal to 0.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                return BadRequest(new { Message = "Page number must be greater than 0." });

            var doctors = await _doctorService.GetAllAsync(pageNumber, pageSize);
            return Ok(doctors);
        }

        /// <summary>
        /// Gets a doctor by ID.
        /// </summary>
        /// <param name="id">The ID of the doctor.</param>
        /// <returns>The doctor with the specified ID.</returns>
        /// <response code="200">Returns the doctor with the specified ID.</response>
        /// <response code="404">If the doctor is not found.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(string id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);

            if (doctor == null)
            {
                return NotFound(new { Message = "Doctor not found" });
            }

            return Ok(doctor);
        }

        /// <summary>
        /// Adds a new doctor.
        /// </summary>
        /// <param name="doctorDto">The doctor data transfer object.</param>
        /// <returns>The created doctor.</returns>
        /// <response code="201">Returns the newly created doctor.</response>
        /// <response code="400">If the doctor data is invalid.</response>
        [Authorize("Admin")]
        [HttpPost]
   
        public async Task<IActionResult> AddDoctor([FromBody] DoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var specialization = await _specializationService.GetByNameAsync(doctorDto.Specialization);
            if (specialization == null)
            {
                return BadRequest(new { Message = "Invalid specialization." });
            }

            var newDoctor = new Doctor
            {
                ApplicationUserId = doctorDto.Id,
                Bio = doctorDto.Bio,
                ProfilePictureUrl = doctorDto.ProfilePictureUrl,
                SpecializationId = specialization.Id,
                GovernorateId = doctorDto.GovernorateId,
            };

            var addedDoctor = await _doctorService.AddAsync(newDoctor);

            return CreatedAtAction(nameof(GetDoctorById), new { id = addedDoctor.ApplicationUserId }, doctorDto);
        }

        /// <summary>
        /// Updates an existing doctor.
        /// </summary>
        /// <param name="id">The ID of the doctor to update.</param>
        /// <param name="doctorDto">The updated doctor data transfer object.</param>
        /// <returns>The updated doctor.</returns>
        /// <response code="200">Returns the updated doctor.</response>
        /// <response code="400">If the doctor ID is null or empty, or if the doctor data is invalid.</response>
        /// <response code="404">If the doctor is not found.</response>
        [Authorize("Doctor")]
        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateDoctor(string id, [FromBody] DoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctorDto.Id)
            {
                return BadRequest(new { Message = "Doctor ID mismatch" });
            }

            var updatedDoctor = await _doctorService.UpdateAsync(doctorDto);

            if (updatedDoctor == null)
            {
                return NotFound(new { Message = "Doctor not found" });
            }

            return Ok(updatedDoctor);
        }

        /// <summary>
        /// Deletes a doctor by ID.
        /// </summary>
        /// <param name="id">The ID of the doctor to delete.</param>
        /// <returns>No content if the deletion was successful.</returns>
        /// <response code="204">If the doctor was successfully deleted.</response>
        /// <response code="404">If the doctor is not found.</response>
        [Authorize("Admin")]
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteDoctor(string id)
        {
            var isDeleted = await _doctorService.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound(new { Message = "Doctor not found" });
            }

            return NoContent();
        }

        /// <summary>
        /// Searches for doctors based on specified criteria.
        /// </summary>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>A list of doctors that match the search criteria.</returns>
        /// <response code="200">Returns the list of doctors that match the search criteria.</response>
        [HttpGet("search")]
        public async Task<IActionResult> SearchDoctors([FromQuery] DoctorSearchCriteria criteria)
        {
            var doctors = await _doctorService.SearchDoctorsAsync(criteria);
            return Ok(doctors);
        }
    }
}