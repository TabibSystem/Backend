using TabibApp.Application.Dtos;

namespace TabibApp.Application.Interfaces;

public interface ISpecializationService
{
    Task<Specialization?> GetByNameAsync(string name);
    Task<Specialization?> GetByIDAsync(int id);
    Task<IEnumerable<SpecializationDto>> GetAllAsync();
    Task<SpecializationDto> AddAsync(SpecializationDto specializationDto);
    Task<bool> DeleteAsync(int id);

}