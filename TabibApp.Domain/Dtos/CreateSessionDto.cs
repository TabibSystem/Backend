namespace TabibApp.Application.Dtos;

public class CreateSessionDto
{
    public string Diagnosis { get; set; }
    public string DoctorId { get; set; }
    public string PatientId { get; set; }
    public List<MedicineDto> Medicines { get; set; }
}