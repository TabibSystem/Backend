namespace TabibApp.Application.Dtos;

public class CreateClinicRequest
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Description { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    public TimeSpan SlotDuration { get; set; }
    public ClinicAddressDto ClinicAddress { get; set; }
    public int GovernorateId { get; set; }
}