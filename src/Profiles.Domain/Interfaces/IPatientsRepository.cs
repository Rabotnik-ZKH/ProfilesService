using Profiles.Contracts.Pagination;
using Profiles.Domain.Entities;

namespace Profiles.Domain.Interfaces;

public interface IPatientsRepository
{
    public Task<List<Patient>> GetAllAsync(PatientsQueryParameters queryParameters);

    public Task<Patient?> GetByIdAsync(Guid id);

    public Task CreateAsync(Patient patient);

    public Task UpdateAsync(Patient patient);

    public Task DeleteAsync(Patient patient);
}
