public class Doctor
{
    public int Id { get; set; }

    public string ProfilePictureUrl { get; set; }
    public string Bio { get; set; }
    public bool IsVerfied { get; set; }
    public string WorkCertificateUrl { get; set; }

    public int Age { get; set; }
    public bool Gender { get; set; }
    public int SpecializationId { get; set; }
    public Specialization Specialization { get; set; }
    public int GovernorateId { get; set; }
    public Governorate Governorate { get; set; }

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<Prescription> Prescriptions { get; set; }
    public ICollection<Message> Messages { get; set; }
    public ICollection<Assistant> Assistants { get; set; }
    public ICollection<Patient> Patients { get; set; }
    public ICollection<Clinic> Clinics { get; set; }
    public ICollection<AppointmentSlot> AppointmentSlots { get; set; }



}
