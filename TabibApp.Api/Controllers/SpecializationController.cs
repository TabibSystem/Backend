using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;

namespace TabibApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly ISpecializationService _specializationService;

        public SpecializationController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }

        /// <summary>
        /// Adds a new specialization.
        /// </summary>
        /// <param name="specializationDto">The specialization data transfer object.</param>
        /// <returns>The created specialization.</returns>
        /// <response code="201">Returns the newly created specialization.</response>
        /// <response code="400">If the specialization data is invalid.</response>
        /// <response code="409">If there is a conflict with the existing data.</response>
        [Authorize("Admin")]
        [HttpPost]
        public async Task<IActionResult> AddSpecialization([FromBody] SpecializationDto specializationDto)
        {
            if (specializationDto == null || string.IsNullOrEmpty(specializationDto.Name))
            {
                return BadRequest(new { Message = "Invalid specialization data" });
            }

            try
            {
                var addedSpecialization = await _specializationService.AddAsync(specializationDto);
                return CreatedAtAction(nameof(GetSpecializationById), new { id = addedSpecialization.Id }, addedSpecialization);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Gets all specializations.
        /// </summary>
        /// <returns>A list of specializations.</returns>
        /// <response code="200">Returns the list of specializations.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllSpecializations()
        {
            var specializations = await _specializationService.GetAllAsync();
            return Ok(specializations);
        }

        /// <summary>
        /// Gets a specialization by name.
        /// </summary>
        /// <param name="name">The name of the specialization.</param>
        /// <returns>The specialization with the specified name.</returns>
        /// <response code="200">Returns the specialization with the specified name.</response>
        /// <response code="404">If the specialization is not found.</response>
        [HttpGet("name/{name}")]
        [ProducesResponseType(typeof(SpecializationDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSpecializationByName(string name)
        {
            var specialization = await _specializationService.GetByNameAsync(name);
            if (specialization == null)
            {
                return NotFound(new { Message = "Specialization not found" });
            }

            return Ok(specialization);
        }

        /// <summary>
        /// Gets a specialization by ID.
        /// </summary>
        /// <param name="id">The ID of the specialization.</param>
        /// <returns>The specialization with the specified ID.</returns>
        /// <response code="200">Returns the specialization with the specified ID.</response>
        /// <response code="404">If the specialization is not found.</response>
        [HttpGet("id/{id:int}")]
        public async Task<IActionResult> GetSpecializationById(int id)
        {
            var specialization = await _specializationService.GetByIDAsync(id);
            if (specialization == null)
            {
                return NotFound(new { Message = "Specialization not found" });
            }

            return Ok(specialization);
        }

        /// <summary>
        /// Deletes a specialization by ID.
        /// </summary>
        /// <param name="id">The ID of the specialization to delete.</param>
        /// <returns>No content if the deletion was successful.</returns>
        /// <response code="204">If the specialization was successfully deleted.</response>
        /// <response code="404">If the specialization is not found.</response>
        [Authorize("Admin")]
        [HttpDelete("{id:int}")]

        public async Task<IActionResult> DeleteSpecialization(int id)
        {
            var result = await _specializationService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(new { Message = "Specialization not found" });
            }

            return NoContent();
        }
    }
}