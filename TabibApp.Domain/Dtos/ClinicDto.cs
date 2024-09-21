using System.Text.Json.Serialization;

namespace TabibApp.Application.Dtos;

public class ClinicDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Description { get; set; }
    public int Examination { get; set; }
   
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    public TimeSpan SlotDuration { get; set; } 
    public int ClinicAddressId { get; set; } 
    public ClinicAddressDto ClinicAddress { get; set; }
    public string DoctorId { get; set; }
    
    public int GovernorateId { get; set; }
    
}