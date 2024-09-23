using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TabibApp.Application;
using TabibApp.Application.Dtos;
using TabibApp.Infrastructure.Data;
namespace TabibApp.Infrastructure.Repository;

public class AppointmentRepository:IAppointmentRepository
{
    private readonly AppDbContext _context;
    private readonly IChatRepository _chatRepository;
    private readonly ILogger<AppointmentRepository> _logger;

    public AppointmentRepository(AppDbContext context,IChatRepository chatRepository,ILogger<AppointmentRepository> logger)
    {
        _context = context;
        _chatRepository = chatRepository;
        _logger = logger;
    }

    public async Task<string> GetDoctorId(int DoctorId)
    {
        var doctorId =await  _context.Doctors.FirstOrDefaultAsync(d => d.Id == DoctorId);
        return doctorId.ApplicationUserId;
    } 
    public async Task<string> GetPatientId(int PatientId)
    {
        var patientId =await  _context.Patients.FirstOrDefaultAsync(d => d.Id == PatientId);
        return patientId.ApplicationUserId;
    }
    public async Task<List<AppointmentSlotDto>> GenerateAppointmentSlots(int clinicId, List<DayScheduleDto> daySchedules, TimeSpan duration, string DoctorId)
    {
        
        if (daySchedules == null)
        {
            throw new ArgumentNullException(nameof(daySchedules));
        }

        var clinic = await _context.Clinics.FindAsync(clinicId);
        if (clinic == null)
        {
            throw new Exception("Clinic not found");
        }

        var docId =await _context.Doctors.FirstOrDefaultAsync(d => d.ApplicationUserId == DoctorId);
        var slots = new List<AppointmentSlot>();

        foreach (var daySchedule in daySchedules)
        {
            if (daySchedule == null)
            {
                throw new ArgumentNullException(nameof(daySchedule));
            }

            var currentTime = daySchedule.StartTime;

            while (currentTime + duration <= daySchedule.EndTime)
            {
                slots.Add(new AppointmentSlot
                {
                    StartTime = currentTime,
                    EndTime = currentTime + duration,
                    DayOfWeek = daySchedule.DayOfWeek,
                    ClinicId = clinicId,
                    IsAvailable = true,
                    DoctorId = docId.Id
                });
                
                currentTime += duration;
            }
        }

        _context.AppointmentSlots.AddRange(slots);
        await _context.SaveChangesAsync();
        var slotDtos = slots.Select(slot => new AppointmentSlotDto
        {
            Id= slot.Id,
            StartTime = slot.StartTime,
            EndTime = slot.EndTime,
            DayOfWeek = slot.DayOfWeek,
            ClinicId = slot.ClinicId,
            IsAvailable = slot.IsAvailable,
            DoctorId = slot.DoctorId
        }).ToList();

        return slotDtos;
    }
    
public async Task<AppointmentDto> BookAppointment(BookAppointmentDto appointmentDto)
{
 var appointmentSlot = await _context.AppointmentSlots
        .FirstOrDefaultAsync(slot => slot.Id == appointmentDto.AppointmentSlotId && slot.IsAvailable);

    if (appointmentSlot == null)
    {
        throw new Exception("Appointment slot not available.");
    }

    var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ApplicationUserId == appointmentDto.PatientId);
    if (patient == null)
    {
        throw new Exception("Patient not found.");
    }

    var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.ApplicationUserId == appointmentDto.DoctorId);
    if (doctor == null)
    {
        throw new Exception("Doctor not found.");
    }

    var appointment = new Appointment
    {
        AppointmentDate = DateTime.Today.Add(appointmentSlot.StartTime),
        ReexaminationDate = DateTime.Now.AddDays(10),
        Status = AppointmentStatus.Scheduled,
        DateBooked = DateTime.Now,
        DoctorId = appointmentSlot.DoctorId,
        PatientId = patient.Id,
        AppointmentSlotId = appointmentSlot.Id,
    };

    appointmentSlot.IsAvailable = false;

    _context.Appointments.Add(appointment);

    try
    {
        await _context.SaveChangesAsync(); 

        await _chatRepository.CreatePrivateRoom( appointmentDto.DoctorId, patient.ApplicationUserId,appointment.Id);
    }
    catch (DbUpdateException ex)
    {
        // Log the inner exception for more details
        var innerException = ex.InnerException?.Message;
        _logger.LogError(ex, "An error occurred while saving the entity changes: {InnerException}", innerException);
        throw new Exception($"An error occurred while saving the entity changes: {innerException}", ex);
    }

    return new AppointmentDto
    {
        Id = appointment.Id,
        AppointmentDate = appointment.AppointmentDate,
        Status = appointment.Status,
        DateBooked = appointment.DateBooked,
        DoctorId = doctor.ApplicationUserId,
        PatientId = patient.ApplicationUserId,
        AppointmentSlotId = appointment.AppointmentSlotId,
    };

}
    public async Task<List<AppointmentDto>> GetAppointmentsForPatient(string userId)
    {
        
        var patient= await _context.Patients.FirstOrDefaultAsync(d=>d.ApplicationUserId==userId);
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.ApplicationUserId == userId);
        if (string.IsNullOrEmpty(userId))
        {
            return null;
        }

        if (patient == null)
        {
            return null;
        }
        var appointments = await _context.Appointments
            .Where(a => a.PatientId == patient.Id)
            .ToListAsync();

        return appointments.Select(a => new AppointmentDto
        {
            Id = a.Id,
            AppointmentDate = a.AppointmentDate,
            Status = a.Status,
            DateBooked = a.DateBooked,
            DoctorId = doctor.ApplicationUserId,
            PatientId = patient.ApplicationUserId,
            AppointmentSlotId = a.AppointmentSlotId,
        }).ToList();
    }
    public async Task<List<AppointmentDto>> GetAppointmentsForDoctor(string userId)
    {
        var doctorId = await _context.Doctors.FirstOrDefaultAsync(d=>d.ApplicationUserId==userId);
        if (doctorId is null)
            return null;
        var appointments = await _context.Appointments
            .Where(a => a.DoctorId == doctorId.Id)
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .Include(a => a.AppointmentSlot)
            .ToListAsync();
        return appointments.Select(a => new AppointmentDto
        {
            Id = a.Id,
            AppointmentDate = a.AppointmentDate,
            Status = a.Status,
            DateBooked = a.DateBooked,
            DoctorId = a.Doctor.ApplicationUserId,
            PatientId = a.Patient.ApplicationUserId,
            AppointmentSlotId = a.AppointmentSlotId,
        }).ToList();
    }
    public async Task<bool> CancelAppointment(int appointmentId)
    {
       

        var appointmentSlot = await _context.AppointmentSlots.
            FirstOrDefaultAsync(appointmentslot=>appointmentslot.Id==appointmentId);
        if (appointmentSlot is null)
        {
            return false;
        }
        if (appointmentSlot != null)
        {
            appointmentSlot.IsAvailable = false;
        }

        await _context.SaveChangesAsync();

        return true; 
    }
    public async Task<List<AppointmentSlotDto>> GetAppointmentSlotsByClinicId(int clinicId)
    {
        var slots = await _context.AppointmentSlots
            .Where(slot => slot.ClinicId == clinicId)
            .ToListAsync();

        return slots.Select(slot => new AppointmentSlotDto
        {
            Id = slot.Id,
            StartTime = slot.StartTime,
            EndTime = slot.EndTime,
            DayOfWeek = slot.DayOfWeek,
            ClinicId = slot.ClinicId,
            IsAvailable = slot.IsAvailable, 
            DoctorId = slot.DoctorId
        }).ToList();
    }
}
