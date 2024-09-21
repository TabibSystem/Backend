using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;

namespace TabibApp.Application.Implementation;

public class SpecializationService:ISpecializationService
{
    private readonly ISpecializationRepository _specializationRepository;

    public SpecializationService(ISpecializationRepository specializationRepository)
    {
        _specializationRepository = specializationRepository;
    }
    public  Task<Specialization?> GetByNameAsync(string name)
    {
        return  _specializationRepository.GetByNameAsync(name);

    }

    public  Task<Specialization?> GetByIDAsync(int id)
    {
        return  _specializationRepository.GetByIDAsync(id);
    }

    public  Task<IEnumerable<SpecializationDto>> GetAllAsync()
    {
        return  _specializationRepository.GetAllAsync();
    }

    public  Task<SpecializationDto> AddAsync(SpecializationDto specializationDto)
    {
       return  _specializationRepository.AddAsync(specializationDto);
    }

    public  Task<bool> DeleteAsync(int id)
    {
        return  _specializationRepository.DeleteAsync(id);
    }
}