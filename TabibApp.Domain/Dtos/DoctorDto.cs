namespace TabibApp.Application.Dtos;

public class DoctorDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfilePictureUrl { get; set; }
  //  public string PhoneNumber { get; set; }
    public string Bio { get; set; }
    public string Specialization { get; set; }
    public int SpecializationId { get; set; }
    
    public string Governorate { get; set; }
    public int GovernorateId { get; set; }
    public ICollection<string> Clinics { get; set; }
}