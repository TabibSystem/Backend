namespace TabibApp.Application.Dtos;

public class DoctorAdminDto:DoctorDto
{
    public string PhoneNumber { get; set; }
    public bool IsVerified { get; set; }
    public string WorkCertificateUrl { get; set; }
    public string ProfilePictureUrl { get; set; }
    

}