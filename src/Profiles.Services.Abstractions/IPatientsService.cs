using OneOf;
using OneOf.Types;
using Profiles.Contracts.DTOs.Patient;
using Profiles.Contracts.Pagination;

namespace Profiles.Services.Abstractions;

public interface IPatientsService
{
    public Task<OneOf<List<PatientResponseDTO>, NotFound>> GetAllPatientsAsync(PatientsQueryParameters queryParameters);

    public Task<OneOf<PatientResponseDTO, NotFound>> GetPatientByIdAsync(Guid id);

    public Task<PatientResponseDTO> CreatePatientAsync(PatientCreateDTO newPatient);

    public Task<OneOf<Success, NotFound>> UpdatePatientAsync(Guid id, PatientUpdateDTO updatedPatient);

    public Task<OneOf<Success, NotFound>> DeletePatientAsync(Guid id);
}
