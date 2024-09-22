public class Assistant
{
    public int Id { get; set; }

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }
}
