namespace TabibApp.Application.Dtos;

public class MedicalHistoryResponseDto
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public DateTime UploadDate { get; set; }
}