using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;

namespace TabibApp.Application.Implementation;

public class AppointmentService:IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentService(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public  Task<List<AppointmentSlotDto>> GenerateAppointmentSlots(int clinicId, List<DayScheduleDto> daySchedules, TimeSpan duration, string doctorId)
    {
       return  _appointmentRepository.GenerateAppointmentSlots(clinicId, daySchedules, duration, doctorId);
    }
}