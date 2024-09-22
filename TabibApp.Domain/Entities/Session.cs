namespace TabibApp.Application.Dtos;

public class Session
{
    public int Id { get; set; }
    public string Diagnosis { get; set; }
    public DateTime Date { get; set; }

    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public int PatientId { get; set; }
    public Patient Patient { get; set; }

    public ICollection<Medicine> Medicines { get; set; }
}