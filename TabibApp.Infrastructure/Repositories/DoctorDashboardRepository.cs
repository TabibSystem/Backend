using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TabibApp.Application;
using TabibApp.Infrastructure.Data;

namespace TabibApp.Infrastructure.Repository;

public class DoctorDashboardRepository:IDoctorDashboardRepository
{
    private readonly AppDbContext _context;

        public DoctorDashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DoctorDashboardDto> GetDoctorDashboardDataAsync(string doctorId)
        {
            var doctor = await _context.Doctors
                .Include(d => d.ApplicationUser)
                .FirstOrDefaultAsync(d => d.ApplicationUserId == doctorId);

            if (doctor == null)
            {
                return null;
            }

            var totalPatients = await _context.Appointments
                .Where(a => a.DoctorId == doctor.Id)
                .Select(a => a.PatientId)
                .Distinct()
                .CountAsync();

            var Doctor = _context.Doctors.FirstOrDefault(d => d.ApplicationUserId == doctorId);
            var totalAppointments = await _context.Appointments
                .Where(a => a.DoctorId == Doctor.Id)
                .CountAsync();

            var completedAppointments = await _context.Appointments
                .Where(a => a.DoctorId == Doctor.Id && a.Status == AppointmentStatus.Completed)
                .CountAsync();

            var upcomingAppointments = await _context.Appointments
                .Where(a => a.DoctorId == Doctor.Id && a.AppointmentDate >= DateTime.Now)
                .CountAsync();

            var todayAppointments = await _context.Appointments
                .Where(a => a.DoctorId == Doctor.Id && a.AppointmentDate.Date == DateTime.Today)
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Select(a => new AppointmentDshDto
                {
                    AppointmentId = a.Id,
                    AppointmentDate = a.AppointmentDate,
                    DoctorName = a.Doctor.ApplicationUser.FirstName + " " + a.Doctor.ApplicationUser.LastName,
                    PatientName = a.Patient.ApplicationUser.FirstName + " " + a.Patient.ApplicationUser.LastName,
                    Status = a.Status
                })
                .ToListAsync();

            var doctorClinics = await _context.Clinics
                .Where(d => d.DoctorId == Doctor.Id)
                .Select(c => new ClinicDshDto
                {
                    ClinicId = c.Id,
                    ClinicName = c.Name,
                    Address = c.ClinicAddress.BuildingNumber + " " + c.ClinicAddress.StreetName,
                    TotalAppointments = c.Appointments.Count(a => a.DoctorId == Doctor.Id)
                })
                .ToListAsync();

           

            return new DoctorDashboardDto
            {
                DoctorName = doctor.ApplicationUser.FirstName + " " + doctor.ApplicationUser.LastName,
                TotalPatients = totalPatients,
                TotalAppointments = totalAppointments,
                CompletedAppointments = completedAppointments,
                UpcomingAppointments = upcomingAppointments,
                TodayAppointments = todayAppointments,
                DoctorClinics = doctorClinics,
                UnreadMessages = null
            };
        }
}