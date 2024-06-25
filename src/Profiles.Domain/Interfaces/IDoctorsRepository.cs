using Profiles.Contracts.Pagination;
using Profiles.Domain.Entities;

namespace Profiles.Domain.Interfaces;

public interface IDoctorsRepository
{
    public Task<List<Doctor>> GetAllAsync(DoctorsQueryParameters queryParameters);

    public Task<Doctor?> GetByIdAsync(Guid id);

    public Task CreateAsync(Doctor doctor);

    public Task UpdateAsync(Doctor doctor);

    public Task DeleteAsync(Doctor doctor);
}
