using TabibApp.Application.Dtos;

namespace TabibApp.Application.Interfaces;

public interface IDoctorService
{
    
    Task<IEnumerable<DoctorDto>> GetAllAsync(int pageNumber, int pageSize);
    Task<DoctorDto> GetByIdAsync(string applicationUserId);
    Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(DoctorSearchCriteria criteria);
    Task<Doctor> AddAsync(Doctor doctor);
    Task<DoctorDto> UpdateAsync(DoctorDto doctorDto);
    Task<bool> DeleteAsync(string id);
    Task<int> SaveChangesAsync();

}