using Microsoft.EntityFrameworkCore;
using TabibApp.Application;
using TabibApp.Application.Dtos;
using TabibApp.Infrastructure.Data;

namespace TabibApp.Infrastructure.Repository;

public class ClinicRepository:IClinicRepository
{
    private readonly AppDbContext _context;

    public ClinicRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ClinicExistsAsync(string name, string phoneNumber)
    {
        return await _context.Clinics.AnyAsync(c => c.Name == name && c.PhoneNumber == phoneNumber);
    }

   


    public async Task<Clinic> CreateClinicAsync(Clinic clinic)
    {
        var newClinic = await _context.Clinics.AddAsync(clinic);
        await _context.SaveChangesAsync();
        return newClinic.Entity;
    }

    public async Task<Clinic?> GetClinicById(string id)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.ApplicationUserId == id);
        return await _context.Clinics.FirstOrDefaultAsync(c => c.DoctorId == doctor.Id);
    }

    public async Task<Clinic?> UpdateClinicAsync(int clinicId, Clinic updatedClinic)
    {
        // Retrieve the existing clinic by Id
        var clinic = await _context.Clinics
            .FirstOrDefaultAsync(c => c.Id == clinicId);
    
        // Check if the clinic exists
        if (clinic == null)
        {
            return null; // Return null if the clinic was not found
        }
    
        // Update clinic properties
        clinic.Name = updatedClinic.Name;
        clinic.PhoneNumber = updatedClinic.PhoneNumber;
        clinic.Description = updatedClinic.Description;
        clinic.Examination = updatedClinic.Examination;
        clinic.OpeningTime = updatedClinic.OpeningTime;
        clinic.ClosingTime = updatedClinic.ClosingTime;
        clinic.DoctorId = updatedClinic.DoctorId; // Update DoctorId if needed
        clinic.GovernorateId = updatedClinic.GovernorateId;
    
        // Update clinic address
        if (updatedClinic.ClinicAddress != null)
        {
            clinic.ClinicAddress.BuildingNumber = updatedClinic.ClinicAddress.BuildingNumber;
            clinic.ClinicAddress.StreetName = updatedClinic.ClinicAddress.StreetName;
            clinic.ClinicAddress.Floor = updatedClinic.ClinicAddress.Floor;
            clinic.ClinicAddress.ApartmentNumber = updatedClinic.ClinicAddress.ApartmentNumber;
            clinic.ClinicAddress.Latitude = updatedClinic.ClinicAddress.Latitude;
            clinic.ClinicAddress.Longitude = updatedClinic.ClinicAddress.Longitude;
        }

        // Save changes to the database
        await _context.SaveChangesAsync();
    
        // Return the updated clinic
        return clinic;
        
        
        
    }


    public async Task<bool> DeleteClinicAsync(int  id)
    {
        var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.Id == id);
        if (clinic is null)
            return false;

        _context.Clinics.Remove(clinic);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Clinic>> GetClinicsByDoctorIdAsync(string applicationUserId)
    {
        var doctor = await _context.Doctors
            .Include(d => d.Clinics)
            .FirstOrDefaultAsync(d => d.ApplicationUserId == applicationUserId);

        return doctor?.Clinics ?? new List<Clinic>();
    }

    

    public async Task<int?> GetDoctorIdByUserIdAsync(string applicationUserId)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.ApplicationUserId == applicationUserId);
        if (doctor is null)
            return null;
        return doctor.Id; 
    }

    public async Task SaveScheduleAsync(IEnumerable<TimeSlot> timeSlots)
    {
        throw new NotImplementedException();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}

