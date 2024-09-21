namespace TabibApp.Application.Dtos;

public class TimeSlot
{
    public int Id { get; set; }
    public int ClinicId { get; set; }
    public int DoctorId { get; set; }
    public DayOfWeek Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}