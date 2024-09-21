using TabibApp.Application.Dtos;

public class Clinic
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Description { get; set; }
    public int Examination { get; set; }
   
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }

    public List<DayScheduleDto> DaySchedules { get; set; }
    
    public int ClinicAddressId { get; set; } 
    public ClinicAddress ClinicAddress { get; set; }
    public int DoctorId { get; set; }
    public Doctor OwnerDoctor { get; set; }
    
    public int GovernorateId { get; set; }
    public Governorate Governorate { get; set; }
    
    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<AppointmentSlot> AppointmentSlots { get; set; }
    

}
