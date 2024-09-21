
public class Prescription
{
    public int Id { get; set; }
    public string fileUrl { get; set; }

    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public int PatientId { get; set; }
    public Patient Patient { get; set; }

    public Appointment Appointment { get; set; }
}
