namespace TabibApp.Application;

public interface IDoctorDashboardRepository
{
    Task<DoctorDashboardDto> GetDoctorDashboardDataAsync(string doctorId);
}