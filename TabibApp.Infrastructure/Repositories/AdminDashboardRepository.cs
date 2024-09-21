using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TabibApp.Application;
using TabibApp.Application.Dtos;
using TabibApp.Infrastructure.Data;

namespace TabibApp.Infrastructure.Repository;

public class AdminDashboardRepository:IAdminDashboardRepository
{
    private readonly AppDbContext _context;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public AdminDashboardRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private string ConstructFullUrl(string relativeUrl)
    {
        var request = _httpContextAccessor.HttpContext.Request;
        return $"{request.Scheme}://{request.Host}{relativeUrl}";
    }

    public async Task<IEnumerable<DoctorAdminDto>> GetDoctorsAsync()
    {
        var doctors = await _context.Doctors
            .Include(d => d.ApplicationUser)
            .ToListAsync();

        var doctorDtos = new List<DoctorAdminDto>();

        foreach (var doctor in doctors)
        {
            var doctorDto = new DoctorAdminDto
            {
                Id = doctor.ApplicationUserId,
                FirstName = doctor.ApplicationUser.FirstName,
                LastName = doctor.ApplicationUser.LastName,
                Bio = doctor.Bio,
                IsVerified = doctor.IsVerfied,
                WorkCertificateUrl = ConstructFullUrl(doctor.WorkCertificateUrl),
                ProfilePictureUrl = ConstructFullUrl(doctor.ProfilePictureUrl),
                PhoneNumber = doctor.ApplicationUser.PhoneNumber!
            };

            doctorDtos.Add(doctorDto);
        }

        return doctorDtos;
    }
    
    public async Task<AdminDashboardDto> GetDashboardDataAsync()
    {
        var totalDoctors = await _context.Doctors.CountAsync();
        var totalPatients = await _context.Patients.CountAsync();
        var totalAppointments = await _context.Appointments.CountAsync();
        var totalClinics = await _context.Clinics.CountAsync();

        var pendingDoctorVerifications = await _context.Doctors
            .Where(d => d.IsVerfied == false)
            .Select(d => new DoctorVerificationDto
            {
                DoctorId = d.ApplicationUserId,
                DoctorName = d.ApplicationUser.FirstName +" " + d.ApplicationUser.LastName,
                IsVerified = d.IsVerfied ,
            })
            .ToListAsync();

        return new AdminDashboardDto
        {
            TotalDoctors = totalDoctors,
            TotalPatients = totalPatients,
            TotalAppointments = totalAppointments,
            TotalClinics = totalClinics,
            PendingDoctorVerifications = pendingDoctorVerifications
        };
    }

    public async Task<bool> VerifyDoctor(string doctorId)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d=>d.ApplicationUserId==doctorId);
        if (doctor == null)
            return false;
    
        doctor.IsVerfied = true;
        _context.Doctors.Update(doctor);
        await _context.SaveChangesAsync();
        return true;

    }
  
}