using TabibApp.Application.Dtos;

namespace TabibApp.Application;

public interface IMedicalHistoryRecordsRepository
{
    Task<bool> Add(MedicalHistoryRecord medicalHistoryRecord);
    Task<IEnumerable<MedicalHistoryResponseDto>> Get(string  patientId);
    Task<int?> GetpatientId(string userId);

}