
using Microsoft.AspNetCore.Mvc;
using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;

namespace TabibApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost]
        public async Task<ActionResult<SessionDto>> CreateSession(CreateSessionDto createSessionDto)
        {
            var sessionDto = await _sessionService.AddSessionAsync(createSessionDto);
            if (sessionDto is null)
                return NotFound("The Patient Not Found");
            return Ok(sessionDto);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<SessionDto>>> GetSessionsByPatientId(string patientId)
        {
            var sessions = await _sessionService.GetSessionsByPatientIdAsync(patientId);
            if(sessions is null)
                return NotFound("Patient Not Found");
            return Ok(sessions);
        }

       

        [HttpGet("doctor/{doctorId}/patientname/{patientName}")]
        public async Task<ActionResult<IEnumerable<SessionDto>>> GetSessionsByPatientName(string patientName, string doctorId)
        {
            var sessions = await _sessionService.GetSessionsByPatientNameAsync(patientName, doctorId);
            return Ok(sessions);
        }
    }
}