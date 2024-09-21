using TabibApp.Application.Dtos;

namespace TabibApp.Application.Interfaces;

public interface IAppointmentService
{
   Task<List<AppointmentSlotDto>> GenerateAppointmentSlots(int clinicId, List<DayScheduleDto> daySchedules, TimeSpan duration, string doctorId);
}