using Microsoft.EntityFrameworkCore;
using TabibApp.Application;
using TabibApp.Application.Dtos;
using TabibApp.Infrastructure.Data;

namespace TabibApp.Infrastructure.Repository;

public class SessionRepository:ISessionRepository
{
    private readonly AppDbContext _context;

    public SessionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Session> AddSessionAsync(Session session)
    {
        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task<int> GetDoctorId(string UserID)
    {
        var doctor =await _context.Doctors.FirstOrDefaultAsync(d => d.ApplicationUserId == UserID);
        if (doctor is null)
            return 0;
        return doctor.Id;
    }

    public async Task<int> GetPatientId(string UserID)
    {
        var Patient =await _context.Patients.FirstOrDefaultAsync(d => d.ApplicationUserId == UserID);
        if (Patient is null)
            return 0;
        return Patient.Id;    }

    public async Task<IEnumerable<Session>> GetSessionsByPatientIdAsync(int patientId)
    {
        return await _context.Sessions
            .Include(s => s.Medicines)
            .Include(s => s.Doctor)
            .ThenInclude(d => d.ApplicationUser)
            .Include(s => s.Patient)
            .ThenInclude(p => p.ApplicationUser)
            .Where(s => s.PatientId == patientId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Session>> GetSessionsByPatientNameAsync(string patientName, int doctorId)
    {
        return await _context.Sessions
            .Include(s => s.Medicines)
            .Include(s => s.Doctor)
            .ThenInclude(d => d.ApplicationUser)
            .Include(s => s.Patient)
            .ThenInclude(p => p.ApplicationUser)
            .Where(s => s.DoctorId == doctorId && 
                        (s.Patient.ApplicationUser.FirstName + " " + s.Patient.ApplicationUser.LastName).Contains(patientName))
            .ToListAsync();
    }
    public async Task<Session> GetSessionByIdAsync(int sessionId)
    {
        return await _context.Sessions
            .Include(s => s.Medicines)
            .Include(s => s.Doctor)
            .ThenInclude(d => d.ApplicationUser)
            .Include(s => s.Patient)
            .ThenInclude(p => p.ApplicationUser)
            .FirstOrDefaultAsync(s => s.Id == sessionId);
    }
}