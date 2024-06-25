using Profiles.Domain.Entities;

namespace Profiles.Domain.Interfaces;

public interface IReceptionistsRepository
{
    public Task<List<Receptionist>> GetAllAsync();

    public Task<Receptionist?> GetByIdAsync(Guid id);

    public Task CreateAsync(Receptionist receptionist);

    public Task UpdateAsync(Receptionist receptionist);

    public Task DeleteAsync(Receptionist receptionist);
}
