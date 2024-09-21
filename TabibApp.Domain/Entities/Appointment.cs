public class Appointment
{
    public int Id { get; set; }
    public DateTime AppointmentDate { get; set; }
    public DateTime? ReexaminationDate { get; set; }

    public AppointmentStatus Status { get; set; }
    public DateTime DateBooked { get; set; }
    
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public int PatientId { get; set; }
    public Patient Patient { get; set; }
    public int AppointmentSlotId { get; set; }
    public AppointmentSlot AppointmentSlot { get; set; }
    public int? PrescriptionId { get; set; }
    public Prescription Prescription { get; set; }
    public int? ChatId { get; set; }
    public Chat Chat { get; set; }
    public bool IsChatActive()
    {
        if (ReexaminationDate.HasValue)
        {
            return DateTime.Now >= AppointmentDate && DateTime.Now <= ReexaminationDate.Value;
        }
        return false;
    }
}
