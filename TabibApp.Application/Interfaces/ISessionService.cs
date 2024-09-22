using TabibApp.Application.Dtos;

namespace TabibApp.Application.Interfaces;

public interface ISessionService
{
    Task<SessionDto> AddSessionAsync(CreateSessionDto createSessionDto);
    Task<IEnumerable<SessionDto>> GetSessionsByPatientIdAsync(string patientId);
    Task<IEnumerable<SessionDto>> GetSessionsByPatientNameAsync(string patientName, string doctorId);
}
