  // DTO for the Admin Dashboard
    public class AdminDashboardDto
    {
        public int TotalDoctors { get; set; }
        public int TotalPatients { get; set; }
        public int TotalAppointments { get; set; }
        public int TotalClinics { get; set; }
        public List<DoctorVerificationDto> PendingDoctorVerifications { get; set; }
      
    }
  
    // DTO for Doctor Dashboard
    public class DoctorDashboardDto
    {
        public string DoctorName { get; set; }
        public int TotalPatients { get; set; }
        public int TotalAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int UpcomingAppointments { get; set; }
        public List<AppointmentDshDto> TodayAppointments { get; set; }
        public List<ClinicDshDto> DoctorClinics { get; set; }
        public List<MessageDshDto> UnreadMessages { get; set; }
    }
  
    // DTO for Patient Dashboard
    public class PatientDashboardDto
    {
        public string PatientName { get; set; }
        public int TotalAppointments { get; set; }
        public List<AppointmentDshDto> UpcomingAppointments { get; set; }
        public List<AppointmentDshDto> MedicalHistory { get; set; }
        public List<MessageDshDto> UnreadMessages { get; set; }
        public List<NotificationDshDto> RecentNotifications { get; set; }
    }
  
    // Supporting DTOs
    public class DoctorVerificationDto
    {
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public bool IsVerified { get; set; }
    }
  
    public class AppointmentDshDto
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public AppointmentStatus Status { get; set; }
    }
  
    public class ClinicDshDto
    {
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string Address { get; set; }
        public int TotalAppointments { get; set; }
    }
  
    public class MessageDshDto
    {
        public int MessageId { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
  
    public class NotificationDshDto
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
  
  
  
