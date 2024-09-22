namespace TabibApp.Application.Dtos;

public class SessionDto
{
    public int Id { get; set; }
    public string Diagnosis { get; set; }
    public DateTime Date { get; set; }
    public string DoctorId { get; set; }
    public string PatientId { get; set; }
    public List<MedicineDto> Medicines { get; set; }
}