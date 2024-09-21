namespace TabibApp.Application.Dtos;

public class AppointmentDto
{
    public int Id { get; set; } 
    public DateTime AppointmentDate { get; set; } 
    public AppointmentStatus Status { get; set; } 
    public DateTime DateBooked { get; set; } 
    public string DoctorId { get; set; } 
    public string PatientId { get; set; } 
    public int AppointmentSlotId { get; set; } 
}