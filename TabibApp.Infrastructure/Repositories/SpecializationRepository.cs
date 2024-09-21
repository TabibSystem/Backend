using Microsoft.EntityFrameworkCore;
using TabibApp.Application;
using TabibApp.Application.Dtos;
using TabibApp.Infrastructure.Data;

namespace TabibApp.Infrastructure.Repository;

public class SpecializationRepository:ISpecializationRepository
{
    private readonly AppDbContext _context;

    public SpecializationRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Specialization?> GetByNameAsync(string name)
    {
        return await _context.Specializations
            .FirstOrDefaultAsync(s => s.Name == name);
    }

    public async Task<Specialization?> GetByIDAsync(int id)
    {
        return await _context.Specializations.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<SpecializationDto>> GetAllAsync()
    {
        return await _context.Specializations.Select(s=> new SpecializationDto{Id = s.Id,Name = s.Name}).ToListAsync();
    }

    public async Task<SpecializationDto> AddAsync(SpecializationDto specializationDto)
    {
        var existingSpecialization = await _context.Specializations
            .FirstOrDefaultAsync(s => s.Name == specializationDto.Name);

        if (existingSpecialization != null)
        {
            throw new InvalidOperationException("Specialization already exists");
        }

        var specialization = new Specialization
        {
            Name = specializationDto.Name
        };

        await _context.Specializations.AddAsync(specialization);
        await _context.SaveChangesAsync();

        return new SpecializationDto
        {
            Id = specialization.Id,
            Name = specialization.Name
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var specialization = await _context.Specializations
            .FirstOrDefaultAsync(s => s.Id == id);

        if (specialization == null)
        {
            return false;
        }

        _context.Specializations.Remove(specialization);
        await _context.SaveChangesAsync();
        return true;
    }
}