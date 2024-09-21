using TabibApp.Application.Dtos;

namespace TabibApp.Application;

public interface IPatientRepository
{
    Task<IEnumerable<PatientDto>> GetAll();
    Task<PatientDto> GetById(string id);
    Task<Patient> Add(Patient entity);
    Task<PatientDto> Update(PatientDto entity);
    Task<bool> Delete(string id);
    Task<int> SaveChangesAsync();



}