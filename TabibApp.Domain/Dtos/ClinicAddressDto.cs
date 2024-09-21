namespace TabibApp.Application.Dtos;

public class ClinicAddressDto
{
    public string BuildingNumber { get; set; }
    public string StreetName { get; set; }
    public string? Floor { get; set; }
    public string? ApartmentNumber { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}