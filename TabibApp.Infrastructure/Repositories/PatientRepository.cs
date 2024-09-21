using Microsoft.EntityFrameworkCore;
using TabibApp.Application;
using TabibApp.Application.Dtos;
using TabibApp.Infrastructure.Data;

namespace TabibApp.Infrastructure.Repository;

public class PatientRepository:IPatientRepository
{
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<PatientDto>> GetAll()
    {
        var patients = await _context.Patients
            .Include(p => p.ApplicationUser)
            .Select(p => new PatientDto
            {
                Id = p.ApplicationUserId,
                FirstName = p.ApplicationUser.FirstName,
                LastName = p.ApplicationUser.LastName,
                PhoneNumber = p.ApplicationUser.PhoneNumber,
                Age = p.Age,
                Gender = p.Gender
            })
            .ToListAsync();
        return patients;

    }

    public async Task<PatientDto> GetById(string id)
    {
        var patient = await _context.Patients
            .Include(p => p.ApplicationUser)
            .Where(p => p.ApplicationUserId == id)
            .Select(p => new PatientDto
            {
                Id = p.ApplicationUserId,
                FirstName = p.ApplicationUser.FirstName,
                LastName = p.ApplicationUser.LastName,
                PhoneNumber = p.ApplicationUser.PhoneNumber,
                Age = p.Age,
                Gender = p.Gender
            }).FirstOrDefaultAsync();
        return patient;
    }

    public async Task<Patient> Add(Patient entity)
    {
       await _context.Patients.AddAsync(entity);
       return entity;
    }

    public async Task<PatientDto> Update(PatientDto entity)
    {
        var patient =await  _context.Patients
            .Include(p => p.ApplicationUser)
            .FirstOrDefaultAsync(U => U.ApplicationUserId == entity.Id);

        if (patient == null)
        {
            throw new Exception("Patient not found");
        }

        patient.ApplicationUser.FirstName = entity.FirstName;
        patient.ApplicationUser.LastName = entity.LastName;
        patient.ApplicationUser.PhoneNumber = entity.PhoneNumber;
        patient.Age = entity.Age;
        patient. Age = entity.Age;
        patient. Gender = entity.Gender;

        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<bool> Delete(string id)
    {
        var patient = await _context.Patients.Include(p=>p.ApplicationUser)
            .FirstOrDefaultAsync(p=>p.ApplicationUserId == id);
        if (patient == null)
        {
            return false;
        }

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}