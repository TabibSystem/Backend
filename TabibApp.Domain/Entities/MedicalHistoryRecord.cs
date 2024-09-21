
public class MedicalHistoryRecord
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FileUrl  { get; set; }
    public DateTime UploadDate { get; set; }

    public int PatientId { get; set; }
    public Patient Patient { get; set; }
}