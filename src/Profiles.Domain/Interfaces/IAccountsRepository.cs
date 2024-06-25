using Profiles.Domain.Entities;

namespace Profiles.Domain.Interfaces;

public interface IAccountsRepository
{
    public Task DeleteAsync(Account account);
}
