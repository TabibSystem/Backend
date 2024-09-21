using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TabibApp.Application;
using TabibApp.Application.Dtos;
using TabibApp.Infrastructure.Data;

namespace TabibApp.Infrastructure.Repository
{
    public class MedicalHistoryRecordsRepository : IMedicalHistoryRecordsRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MedicalHistoryRecordsRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int?> GetpatientId(string userId)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ApplicationUserId == userId);
            if (patient == null)
            {
                return null;
            }
            return patient.Id;
        }

        public async Task<bool> Add(MedicalHistoryRecord medicalHistoryRecord)
        {
            await _context.MedicalHistoryRecords.AddAsync(medicalHistoryRecord);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<MedicalHistoryResponseDto>> Get(string patientId)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ApplicationUserId == patientId);
            if (patient == null)
            {
                return Enumerable.Empty<MedicalHistoryResponseDto>();
            }

            var records = await _context.MedicalHistoryRecords
                .Where(m => m.PatientId == patient.Id)
                .ToListAsync();

            var responseDtos = records.Select(m => new MedicalHistoryResponseDto
            {
                Id = m.Id,
                FileName = m.FileName,
                FileUrl = ConstructFullUrl(m.FileUrl),
                UploadDate = m.UploadDate
            }).ToList();

            return responseDtos;
        }

        private string ConstructFullUrl(string relativeUrl)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}{relativeUrl}";
        }
    }
}