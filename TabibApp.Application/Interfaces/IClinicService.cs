using TabibApp.Application.Dtos;

namespace TabibApp.Application.Interfaces;

public interface IClinicService
{
    Task<ClinicDto> CreateClinicAsync(ClinicDto clinicDto);
    Task SaveScheduleAsync(IEnumerable<TimeSlot> timeSlots);
    Task<bool> ClinicExistsAsync(string name, string phoneNumber);
    Task<ClinicDto?> GetClinicById(string id);
    Task<IEnumerable<Clinic>> GetClinicsByDoctorIdAsync(string applicationUserId);
    
    Task<Clinic?> UpdateClinicAsync(int clinicId, ClinicDto updatedClinic);
    Task<bool> DeleteClinicAsync(int id); 
    Task<int> SaveChangesAsync();
}