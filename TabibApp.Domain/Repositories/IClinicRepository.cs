using TabibApp.Application.Dtos;

namespace TabibApp.Application;

public interface IClinicRepository
{
    Task<Clinic> CreateClinicAsync(Clinic clinic);
    Task<Clinic?> GetClinicById(string id);
    Task<Clinic?> UpdateClinicAsync(int clinicId, Clinic updatedClinic);
    Task<bool> DeleteClinicAsync(int id); 
    Task<IEnumerable<Clinic>> GetClinicsByDoctorIdAsync(string applicationUserId);
    Task SaveScheduleAsync(IEnumerable<TimeSlot> timeSlots);
    Task<bool> ClinicExistsAsync(string name, string phoneNumber);
    Task<int?> GetDoctorIdByUserIdAsync(string applicationUserId);
    Task<int> SaveChangesAsync();
    
}