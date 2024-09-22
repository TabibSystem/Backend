using TabibApp.Application.Dtos;

namespace TabibApp.Application;

public interface IAssistantRepository
{
    Task<IEnumerable<AssistantDto>> GetAllAsync(string DoctorId);
    Task<AssistantDto> GetByIdAsync(int id);
    Task<AssistantDto> AddAsync(CreateAssistantDto assistant);

    Task<AssistantDto> UpdateAsync(string Id, CreateAssistantDto updateAssistantDto);
    Task<bool> DeleteAsync(int id);
}