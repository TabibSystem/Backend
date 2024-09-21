using TabibApp.Application.Dtos;

namespace TabibApp.Application;

public interface IAdminDashboardRepository
{
    Task<AdminDashboardDto> GetDashboardDataAsync();
    Task<IEnumerable<DoctorAdminDto>> GetDoctorsAsync();
    Task<bool> VerifyDoctor(string doctorId);
}