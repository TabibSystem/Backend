using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using TabibApp.Application;
using TabibApp.Application.Dtos;
using TabibApp.Infrastructure.Data;

public class AssistantRepository : IAssistantRepository
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AssistantRepository(AppDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<AssistantDto>> GetAllAsync(string DoctorId)
    {
        var assistants = await _context.Assistants
            .Include(a => a.ApplicationUser)
            .Include(a => a.Doctor)
            .Where(a => a.Doctor.ApplicationUserId == DoctorId)
            .ToListAsync();

        var assistantDtos = assistants.Select(a => new AssistantDto
        {
            Id = a.ApplicationUserId,
            FirstName = a.ApplicationUser.FirstName,
            LastName = a.ApplicationUser.LastName,
            Email = a.ApplicationUser.Email,
            DoctorId = a.Doctor.ApplicationUserId
        });

        return assistantDtos;
    }

    public async Task<AssistantDto> GetByIdAsync(int id)
    {
        var assistant = await _context.Assistants
            .Include(a => a.ApplicationUser)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (assistant == null)
        {
            return null;
        }

        var assistantDto = new AssistantDto
        {
            Id = assistant.ApplicationUserId,
            FirstName = assistant.ApplicationUser.FirstName,
            LastName = assistant.ApplicationUser.LastName,
            DoctorId = assistant.Doctor.ApplicationUserId
        };

        return assistantDto;
        
    }

    public async Task<AssistantDto> AddAsync(CreateAssistantDto assistant)
    {
        var user = new ApplicationUser()
        {
            FirstName = assistant.FirstName,
            LastName = assistant.LastName,
            Email = assistant.Email,
            UserName = assistant.Email
            


        };
        var result = await _userManager.CreateAsync(user, assistant.Password);
        if (!result.Succeeded)
        {
            throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // Add the user to the "Assistant" role
        var roleResult = await _userManager.AddToRoleAsync(user, "Assistant");
        if (!roleResult.Succeeded)
        {
            throw new Exception("Failed to add user to role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }

        // Find the doctor by ApplicationUserId
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.ApplicationUserId == assistant.DoctorId);
        if (doctor == null)
        {
            throw new Exception("Doctor not found");
        }
        var createAssitant = new Assistant()
        {
            ApplicationUserId = user.Id,
            DoctorId = doctor.Id,
        };
        _context.Assistants.Add(createAssitant);
        await _context.SaveChangesAsync();
        
        return new AssistantDto()
        {
            FirstName = assistant.FirstName,
            LastName = assistant.LastName,
            DoctorId = assistant.DoctorId,
            Email = assistant.Email,
            
            
        };
    }

    public async Task<AssistantDto> UpdateAsync( string Id,CreateAssistantDto updateAssistantDto)
    {
        var assistant = await _context.Assistants
            .Include(a => a.ApplicationUser)
            .FirstOrDefaultAsync(a => a.ApplicationUser.Id == Id);

        if (assistant == null)
        {
            return null;
        }

        var user = assistant.ApplicationUser;
        if (user == null)
        {
            return null;
        }

        user.FirstName = updateAssistantDto.FirstName;
        user.LastName = updateAssistantDto.LastName;
        user.Email = updateAssistantDto.Email;
        

        _context.Users.Update(user);

        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.ApplicationUserId == updateAssistantDto.DoctorId);

        if (doctor == null)
        {
            throw new Exception("Doctor not found");
        }

      
        assistant.ApplicationUserId = user.Id;
        assistant.DoctorId = doctor.Id;

        _context.Assistants.Update(assistant);
        await _context.SaveChangesAsync();

        var assistantDto = new AssistantDto
        {
            FirstName = assistant.ApplicationUser.FirstName,
            LastName = assistant.ApplicationUser.LastName,
            Email = assistant.ApplicationUser.Email,
            DoctorId = doctor.ApplicationUserId
            
        };

        return assistantDto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var assistant = await _context.Assistants
            .Include(a => a.ApplicationUser)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (assistant == null)
        {
            return false;
        }

        var user = assistant.ApplicationUser;
        if (user != null)
        {
            _context.Users.Remove(user);
        }

        _context.Assistants.Remove(assistant);
        await _context.SaveChangesAsync();
        return true;
    }
}