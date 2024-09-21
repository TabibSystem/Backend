namespace TabibApp.Application.Dtos;

public class DoctorSearchCriteria
{
    public int? SpecializationId { get; set; }

    public int? GovernorateId { get; set; }

    public string? Name { get; set; }

    public double? MinRating { get; set; }
    public double? MaxRating { get; set; }

    public bool? IsVerified { get; set; }
    public bool? Gender { get; set; }
}