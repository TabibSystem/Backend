using TabibApp.Application.Dtos;

namespace TabibApp.Application;

public interface ISessionRepository
{
    Task<Session> AddSessionAsync(Session session);
    Task<int> GetDoctorId(string UserID);
    Task<int> GetPatientId(string UserID);
    Task<IEnumerable<Session>> GetSessionsByPatientIdAsync(int patientId);
    Task<IEnumerable<Session>> GetSessionsByPatientNameAsync(string patientName, int doctorId);

}