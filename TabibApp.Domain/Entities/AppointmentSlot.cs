

public class AppointmentSlot
{
    public int Id { get; set; }
    public TimeSpan  StartTime { get; set; }
    public TimeSpan  EndTime { get; set; }
    public DayOfWeek DayOfWeek { get; set; } 
    
    public TimeSpan Duration { get; set; } 
    public int ClinicId { get; set; }
    public Clinic Clinic { get; set; }
    public int DoctorId { get; set; } 
    public Doctor Doctor { get; set; }
    public bool IsAvailable { get; set; } 
    public ICollection<Appointment> Appointments { get; set; } 

    
}