public class Patient
{
    public int Id { get; set; }
    public ICollection<MedicalHistoryRecord> MedicalHistory { get; set; }
    public int Age { get; set; }
    public bool Gender { get; set; }

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<Prescription> Prescriptions { get; set; }
    public ICollection<Message> Messages { get; set; }
    public ICollection<Doctor> doctors { get; set; }

}
