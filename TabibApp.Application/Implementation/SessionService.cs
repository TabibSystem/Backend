using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;

namespace TabibApp.Application.Implementation;

public class SessionService:ISessionService
{
    private readonly ISessionRepository _sessionRepository;

    public SessionService(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<SessionDto> AddSessionAsync(CreateSessionDto createSessionDto)
    {
        var doctorid = await _sessionRepository.GetDoctorId(createSessionDto.DoctorId);
        if (doctorid==0)
            return null;
        var paitentid=  await _sessionRepository.GetPatientId(createSessionDto.PatientId);

        var session = new Session
        {
            Diagnosis = createSessionDto.Diagnosis,
            Date = DateTime.UtcNow,
            DoctorId = doctorid,
            PatientId = paitentid,
            Medicines = createSessionDto.Medicines.Select(m => new Medicine
            {
                Name = m.Name,
                Description = m.Description
            }).ToList()
        };
        var addedSession = await _sessionRepository.AddSessionAsync(session);
        return new SessionDto
        {
            Id = addedSession.Id,
            Diagnosis = addedSession.Diagnosis,
            Date = addedSession.Date,
            DoctorId = addedSession.Doctor.ApplicationUserId,
            PatientId = addedSession.Patient.ApplicationUserId,
            Medicines = addedSession.Medicines.Select(m => new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description
            }).ToList()
        };

        
    }

    public async Task<IEnumerable<SessionDto>> GetSessionsByPatientIdAsync(string userid)
    {
        var Patientid=await _sessionRepository.GetPatientId(userid);
        if(Patientid == 0)
         return null;
        var sessions = await _sessionRepository.GetSessionsByPatientIdAsync(Patientid);
        return sessions.Select(s => new SessionDto
        {
            Id = s.Id,
            Diagnosis = s.Diagnosis,
            Date = s.Date,
            DoctorId = s.Doctor.ApplicationUserId,
            PatientId = s.Patient.ApplicationUserId,
            Medicines = s.Medicines.Select(m => new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description
            }).ToList()
        }).ToList();

        
    }

    public async Task<IEnumerable<SessionDto>> GetSessionsByPatientNameAsync(string patientName, string doctorId)
    {
        var Id= await _sessionRepository.GetDoctorId(doctorId);
        if (Id == 0)
            return null;
        var sessions = await _sessionRepository.GetSessionsByPatientNameAsync(patientName, Id);

        return sessions.Select(s => new SessionDto
        {
            Id = s.Id,
            Diagnosis = s.Diagnosis,
            Date = s.Date,
            DoctorId = s.Doctor.ApplicationUserId,
            PatientId = s.Patient.ApplicationUserId,
            Medicines = s.Medicines.Select(m => new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description
            }).ToList()
        }).ToList();


    }
}