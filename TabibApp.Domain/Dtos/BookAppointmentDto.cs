namespace TabibApp.Application.Dtos;

public class BookAppointmentDto
{
    public int clinicId { get; set; }
    public string PatientId { get; set; }             
    public string DoctorId { get; set; }             
    public int AppointmentSlotId { get; set; } 

}