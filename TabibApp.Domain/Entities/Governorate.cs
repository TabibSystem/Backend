public class Governorate
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Doctor> Doctors { get; set; }
    public ICollection<Clinic> Clinics { get; set; }

}
