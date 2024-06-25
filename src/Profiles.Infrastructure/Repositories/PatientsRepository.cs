using Microsoft.EntityFrameworkCore;
using Profiles.Contracts.Pagination;
using Profiles.Domain.Entities;
using Profiles.Domain.Interfaces;
using Profiles.Infrastructure.Data;

namespace Profiles.Infrastructure.Repositories;

public class PatientsRepository : IPatientsRepository
{
    private readonly ProfilesDbContext _context;

    public PatientsRepository(ProfilesDbContext context) => 
        _context = context;

    public async Task<List<Patient>> GetAllAsync(PatientsQueryParameters queryParameters) =>
        await _context.Patients.AsNoTracking().Search(queryParameters.SearchFullName)
            .Include(p => p.Account).ToListAsync();

    public async Task<Patient?> GetByIdAsync(Guid id) =>
        await _context.Patients.AsNoTracking().Include(p => p.Account)
            .FirstOrDefaultAsync(p => p.Id.Equals(id));

    public async Task CreateAsync(Patient newPatient)
    {
        _context.Patients.Add(newPatient);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Patient patient)
    {
        _context.Patients.Update(patient);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Patient patient)
    {
        _context.Remove(patient);

        await _context.SaveChangesAsync();
    }
}
