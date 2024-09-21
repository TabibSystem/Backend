using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TabibApp.Application;

namespace TabibApp.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DoctorDashboardController : Controller
{
    private readonly IDoctorDashboardRepository _doctorDashboardRepository;

    public DoctorDashboardController(IDoctorDashboardRepository doctorDashboardRepository)
    {
        _doctorDashboardRepository = doctorDashboardRepository;
    }

    /// <summary>
    /// Retrieves the doctor's dashboard data.
    /// </summary>
    /// <param name="doctorId">The ID of the doctor.</param>
    /// <returns>Returns the doctor's dashboard data.</returns>
    /// <response code="200">Returns the doctor's dashboard data.</response>
    [Authorize("Doctor")]
    [HttpGet("dashboard/{doctorId}")]
    public async Task<ActionResult<DoctorDashboardDto>> GetDoctorDashboardDataAsync(string doctorId)
    {
        var dashboardData = await _doctorDashboardRepository.GetDoctorDashboardDataAsync(doctorId);
        if (dashboardData == null)
        {
            return NotFound();
        }
        return Ok(dashboardData);
    }
}
