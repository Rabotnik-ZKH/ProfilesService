using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using OneOf.Types;
using Profiles.Contracts.DTOs.Patient;
using Profiles.Contracts.Pagination;
using Profiles.Domain.Entities;
using Profiles.Domain.Interfaces;
using Profiles.Services.Abstractions;

namespace Profiles.Services.Services;

public class PatientsService : IPatientsService
{
    private readonly IAccountsRepository _accountsRepository;
    private readonly IPatientsRepository _patientsRepository;
    private readonly IMapper _mapper;

    public PatientsService(IPatientsRepository patientsRepository, IAccountsRepository accountsRepository, 
        IMapper mapper)
    {
        _accountsRepository = accountsRepository;
        _patientsRepository = patientsRepository;
        _mapper = mapper;
    }

    public async Task<OneOf<List<PatientResponseDTO>, NotFound>> GetAllPatientsAsync(PatientsQueryParameters queryParameters)
    {
        var patients = await _patientsRepository.GetAllAsync(queryParameters);

        if (patients.IsNullOrEmpty())
        {
            return new NotFound();
        }

        var mappedPatients = _mapper.Map<List<PatientResponseDTO>>(patients);

        return mappedPatients;
    }

    public async Task<OneOf<PatientResponseDTO, NotFound>> GetPatientByIdAsync(Guid id)
    {
        var patient = await _patientsRepository.GetByIdAsync(id);

        if (patient is null)
        {
            return new NotFound();
        }

        var mappedPatient = _mapper.Map<PatientResponseDTO>(patient);

        return mappedPatient;
    }

    public async Task<PatientResponseDTO> CreatePatientAsync(PatientCreateDTO newPatient)
    {
        var newPatientEntity = _mapper.Map<Patient>(newPatient);

        newPatientEntity.Account = new Account()
        {
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            PhoneNumber = newPatient.PhoneNumber,
        };

        await _patientsRepository.CreateAsync(newPatientEntity);

        var patientResult = _mapper.Map<PatientResponseDTO>(newPatientEntity);

        return patientResult;
    }

    public async Task<OneOf<Success, NotFound>> UpdatePatientAsync(Guid id, PatientUpdateDTO updatedPatient)
    {
        var patientEntity = await _patientsRepository.GetByIdAsync(id);

        if (patientEntity is null)
        {
            return new NotFound();
        }

        _mapper.Map(updatedPatient, patientEntity);

        await _patientsRepository.UpdateAsync(patientEntity);

        return new Success();
    }

    public async Task<OneOf<Success, NotFound>> DeletePatientAsync(Guid id)
    {
        var patientEntity = await _patientsRepository.GetByIdAsync(id);

        if (patientEntity is null)
        {
            return new NotFound();
        }

        await _accountsRepository.DeleteAsync(patientEntity.Account);

        return new Success();
    }
}
