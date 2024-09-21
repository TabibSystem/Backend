using TabibApp.Application.Dtos;

namespace TabibApp.Application.Interfaces;

public interface IMedicalHistoryService
{
    Task<bool> Add(MedicalHistoryRecord medicalHistoryRecord);
    Task<IEnumerable<MedicalHistoryResponseDto>> Get(string  patientId);
    Task<int?> GetpatientId(string userId);
}