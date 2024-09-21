namespace TabibApp.Application.Dtos;

public class AppointmentSlotDto
{
    public int Id { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public int ClinicId { get; set; }
    public bool IsAvailable { get; set; }
    public int DoctorId { get; set; }
}