using TabibApp.Application.Dtos;

namespace TabibApp.Application;

public interface IDoctorRepository
{
    Task<IEnumerable<DoctorDto>> GetAll(int pageNumber, int pageSize);
    Task<DoctorDto> GetById(string id);
    Task<Doctor> Add(Doctor entity);
    Task<DoctorDto> Update(DoctorDto entity);
    Task<bool> Delete(string id);
    Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(DoctorSearchCriteria criteria);

    Task<int> SaveChangesAsync();
   
}