using TabibApp.Application.Dtos;

namespace TabibApp.Application;

public interface IAppointmentRepository
{
    Task<List<AppointmentSlotDto>> GenerateAppointmentSlots(int clinicId, List<DayScheduleDto> daySchedules, TimeSpan duration, string doctorId);
    Task<AppointmentDto> BookAppointment(BookAppointmentDto appointmentDto);
    Task<bool> CancelAppointment(int appointmentId);
    Task<List<AppointmentDto>> GetAppointmentsForDoctor(string userId);
    Task<List<AppointmentDto>> GetAppointmentsForPatient(string userId);
    Task<List<AppointmentSlotDto>> GetAppointmentSlotsByClinicId(int clinicId);

}