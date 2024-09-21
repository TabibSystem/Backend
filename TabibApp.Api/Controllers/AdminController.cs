using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TabibApp.Application;
using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;
using TabibApp.Infrastructure.Data;

namespace TabibApp.Api.Controllers;
[Authorize("Admin")]

[ApiController]
[Route("api/[controller]")]
public class AdminController : Controller
{
    private readonly IAdminDashboardRepository _adminDashboardRepository;



    public AdminController( IAdminDashboardRepository adminDashboardRepository)
    {
        _adminDashboardRepository = adminDashboardRepository;
    }
    
    /// <summary>
    /// Retrieves the admin dashboard data.
    /// </summary>
    /// <returns>An <see cref="AdminDashboardInsights"/> containing the total number of doctors, patients, appointments, clinics, and a list of pending doctor verifications.</returns>
    [HttpGet("dashboard")]
    public async Task<ActionResult<AdminDashboardDto>> GetDashboardDataAsync()
    {
        var dashboardData = await _adminDashboardRepository.GetDashboardDataAsync();
        return Ok(dashboardData);
    }
    /// <summary>
    /// Retrieves a list of doctors.
    /// </summary>
    /// <response code="200">Returns the list of doctors.</response>
    [HttpGet("doctors")]
    public async Task<ActionResult<IEnumerable<DoctorAdminDto>>> GetDoctorsAsync()
    {
        var doctors = await _adminDashboardRepository.GetDoctorsAsync();
        return Ok(doctors);
    }

    /// <summary>
    /// Verifies a doctor by their ID.
    /// </summary>
    /// <response code="200">If the doctor was successfully verified.</response>
    /// <response code="404">If the doctor with the specified ID was not found.</response>
    [HttpPost("verify-doctor/{doctorId}")]
    public async Task<IActionResult> VerifyDoctor(string doctorId)
    {
        if (await _adminDashboardRepository.VerifyDoctor(doctorId) is false)
            return NotFound();
        return Ok();
    }
}