using Microsoft.EntityFrameworkCore;
using Profiles.Contracts.Pagination;
using Profiles.Domain.Entities;
using Profiles.Domain.Interfaces;
using Profiles.Infrastructure.Data;

namespace Profiles.Infrastructure.Repositories;

public class ReceptionistsRepository : IReceptionistsRepository
{
    private readonly ProfilesDbContext _context;
    
    public ReceptionistsRepository(ProfilesDbContext context) => 
        _context = context;

    public async Task<List<Receptionist>> GetAllAsync(ReceptionistsQueryParameters parameters) =>
        await _context.Receptionists.AsNoTracking()
            .FilterReceptionistsByOffice(parameters.OfficeId)
            .Include(r => r.Account)
            .ToListAsync();

    public async Task<Receptionist?> GetByIdAsync(Guid receptionistId) =>
        await _context.Receptionists.AsNoTracking().Include(r => r.Account)
            .FirstOrDefaultAsync(r => r.Id.Equals(receptionistId));

    public async Task CreateAsync(Receptionist newReceptionist)
    {
        _context.Receptionists.Add(newReceptionist);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Receptionist receptionist)
    {
        _context.Receptionists.Update(receptionist);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Receptionist receptionist)
    {
        _context.Receptionists.Remove(receptionist);

        await _context.SaveChangesAsync();
    }
}
