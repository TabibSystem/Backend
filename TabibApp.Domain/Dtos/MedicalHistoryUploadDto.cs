using Microsoft.AspNetCore.Http;

namespace TabibApp.Application.Dtos;

public class MedicalHistoryUploadDto
{
     public IFormFile File { get; set; }
     public string PatientId { get; set; }
}