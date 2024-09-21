using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;

namespace TabibApp.Application.Implementation;

public class MedicalHistoryService:IMedicalHistoryService
{
    private readonly IMedicalHistoryRecordsRepository _medicalHistoryRecordsRepository;

    public MedicalHistoryService(IMedicalHistoryRecordsRepository medicalHistoryRecordsRepository)
    {
        _medicalHistoryRecordsRepository = medicalHistoryRecordsRepository;
    }
    public  Task<bool> Add(MedicalHistoryRecord medicalHistoryRecord)
    {
        
        return  _medicalHistoryRecordsRepository.Add(medicalHistoryRecord);
    }

    public  Task<IEnumerable<MedicalHistoryResponseDto>> Get(string patientId)
    {
        return  _medicalHistoryRecordsRepository.Get(patientId);
    }

    public  Task<int?> GetpatientId(string userId)
    {
        return _medicalHistoryRecordsRepository.GetpatientId(userId);
    }
}