using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using TabibApp.Application;
using TabibApp.Application.Dtos;
using TabibApp.Infrastructure.Data;

namespace TabibApp.Infrastructure.Repository;

public class DoctorRepository:IDoctorRepository
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DoctorRepository(AppDbContext context,UserManager<ApplicationUser> userManager,IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<DoctorDto>> GetAll(int pageNumber, int pageSize)
    {
        var doctors = await _context.Doctors
            .Include(d => d.Specialization)
            .Include(d => d.Governorate)
            .Include(d => d.Clinics)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var doctorDtos = new List<DoctorDto>();

        foreach (var doctor in doctors)
        {
            var applicationUser = await _userManager.FindByIdAsync(doctor.ApplicationUserId); // Get ApplicationUser details asynchronously

            var doctorDto = new DoctorDto
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                ProfilePictureUrl = ConstructFullUrl(doctor.ProfilePictureUrl),
                Bio = doctor.Bio,
                Specialization = doctor.Specialization.Name,
                Governorate = doctor.Governorate.Name
            };

            doctorDtos.Add(doctorDto);
        }

        return doctorDtos;
    }

    public async Task<DoctorDto> GetById(string applicationUserId)
    {
        // Step 1: Retrieve the Doctor using the ApplicationUserId
        var doctor = await _context.Doctors
            .Include(d => d.Specialization) // Include related data
            .Include(d => d.Governorate) // Include related data
            .Include(d => d.Clinics) // Include related data
            .FirstOrDefaultAsync(d => d.ApplicationUserId == applicationUserId); // Find doctor by ApplicationUserId

        if (doctor == null)
        {
            return null; 
        }

     
        var applicationUser = await _userManager.FindByIdAsync(applicationUserId);

        if (applicationUser == null)
        {
            return null; // or throw an exception, or handle the error
        }

        
        var doctorDto = new DoctorDto
        {
            Id = applicationUser.Id, // Assuming Id in DoctorDto should be the ApplicationUser Id
            FirstName = applicationUser.FirstName,
            LastName = applicationUser.LastName,
         //   PhoneNumber = applicationUser.PhoneNumber,
            ProfilePictureUrl = ConstructFullUrl(doctor.ProfilePictureUrl),
            Bio = doctor.Bio,
            Specialization = doctor.Specialization.Name,
            Governorate = doctor.Governorate.Name


        };

        return doctorDto;

    }
    

    public async Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(DoctorSearchCriteria criteria)
    {
        var query = from doctor in _context.Doctors
            join user in _context.Users on doctor.ApplicationUserId equals user.Id
            join governorate in _context.Governorates on doctor.GovernorateId equals governorate.Id
            select new
            {
                Doctor = doctor,
                User = user,
                Governorate = governorate.Name
            };



        if (criteria.SpecializationId.HasValue)
        {
            query = query.Where(d => d.Doctor.SpecializationId == criteria.SpecializationId.Value);
        }

        if (criteria.GovernorateId.HasValue)
        {
            query = query.Where(d => d.Doctor.GovernorateId == criteria.GovernorateId.Value);
        }

        if (!string.IsNullOrEmpty(criteria.Name))
        {
            query = query.
                Where(d => d.User.FirstName.Contains(criteria.Name) || d.User.LastName.Contains(criteria.Name));
        }
        
        if (criteria.IsVerified.HasValue)
        {
            query = query.Where(d => d.Doctor.IsVerfied == criteria.IsVerified.Value);
        }

        if (criteria.Gender.HasValue)
        {
            query = query.Where(d => d.Doctor.Gender == criteria.Gender);
        }

       
        
        return  await query.Select(d =>new DoctorDto
        {
            Id = d.User.Id,
            FirstName = d.User.FirstName,
            LastName = d.User.LastName,
            Bio = d.Doctor.Bio,
            ProfilePictureUrl = d.Doctor.ProfilePictureUrl,
            Specialization = d.Doctor.Specialization.Name,
            Governorate = d.Doctor.Governorate.Name,
            Clinics = d.Doctor.Clinics.Select(c => c.Name).ToList(),
            
        }).ToListAsync();
    }
    public async Task<Doctor> Add(Doctor doctor)
    {
      await  _context.Doctors.AddAsync(doctor);
      return doctor;
    }

    public async Task<DoctorDto> Update(DoctorDto doctorDto)
    {
       
        var doctor = await _context.Doctors
            .Include(d => d.Specialization)
            .Include(d => d.Governorate)
            .Include(d => d.Clinics).Include(doctor => doctor.ApplicationUser)
            .FirstOrDefaultAsync(d => d.ApplicationUserId == doctorDto.Id);

        if (doctor == null)
        {
            throw new KeyNotFoundException("Doctor not found");
        }

        doctor.ApplicationUser.FirstName = doctorDto.FirstName;
        doctor.ApplicationUser.LastName = doctorDto.LastName;
    
        doctor.Bio = doctorDto.Bio;
    
        // Update Specialization
        var specialization = await _context.Specializations
            .FirstOrDefaultAsync(s => s.Name == doctorDto.Specialization);
        if (specialization != null)
        {
            doctor.SpecializationId = specialization.Id;
        }

        var governorate = await _context.Governorates
            .FirstOrDefaultAsync(g => g.Name == doctorDto.Governorate);
        if (governorate != null)
        {
            doctor.GovernorateId = governorate.Id;
        }


        await _context.SaveChangesAsync();

        return doctorDto;
    }

    public async Task<bool> Delete(string id)
    {
        
        var doctor = await _context.Doctors
            .Include(d => d.Assistants)
            .Include(d => d.Appointments)
            .Include(d => d.Clinics)
            .Include(d => d.Messages)
            .Include(d => d.ApplicationUser)  // Include the related user

            .FirstOrDefaultAsync(d => d.ApplicationUserId == id);

        if (doctor == null)
        {
            return false; 
        }


        _context.Assistants.RemoveRange(doctor.Assistants);

        _context.Appointments.RemoveRange(doctor.Appointments);

        _context.Clinics.RemoveRange(doctor.Clinics);

        _context.Messages.RemoveRange(doctor.Messages);

        _context.Doctors.Remove(doctor);

        _context.Users.Remove(doctor.ApplicationUser);

        await _context.SaveChangesAsync();
        return true;
    }

            private string ConstructFullUrl(string relativeUrl)
            {
                var request = _httpContextAccessor.HttpContext.Request;
                return $"{request.Scheme}://{request.Host}{relativeUrl}";
            }
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}