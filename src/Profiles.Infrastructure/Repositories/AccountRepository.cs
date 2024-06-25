using Profiles.Domain.Entities;
using Profiles.Domain.Interfaces;
using Profiles.Infrastructure.Data;

namespace Profiles.Infrastructure.Repositories;

public class AccountRepository : IAccountsRepository
{
    private readonly ProfilesDbContext _context;

    public AccountRepository(ProfilesDbContext context) =>
        _context = context;

    public async Task DeleteAsync(Account account)
    {
        _context.Accounts.Remove(account);

        await _context.SaveChangesAsync();
    }
}
