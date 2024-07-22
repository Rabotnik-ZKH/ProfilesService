using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using OneOf.Types;
using Profiles.Contracts.DTOs.Doctor;
using Profiles.Contracts.Pagination;
using Profiles.Domain.Entities;
using Profiles.Domain.Interfaces;
using Profiles.Services.Abstractions;

namespace Profiles.Services.Services;

public class DoctorsService : IDoctorsService
{
    private readonly IAccountsRepository _accountsRepository;
    private readonly IDoctorsRepository _doctorsRepository;
    private readonly IMapper _mapper;

    public DoctorsService(IAccountsRepository accountsRepository, IDoctorsRepository doctorsRepository,
        IMapper mapper)
    {
        _accountsRepository = accountsRepository;
        _doctorsRepository = doctorsRepository;
        _mapper = mapper;
    }

    public async Task<OneOf<List<DoctorResponseDTO>, NotFound>> GetAllDoctorsAsync(DoctorsQueryParameters queryParameters)
    {
        var doctors = await _doctorsRepository.GetAllAsync(queryParameters);

        if (doctors.IsNullOrEmpty())
        {
            return new NotFound();
        }

        var mappedDoctors = _mapper.Map<List<DoctorResponseDTO>>(doctors);

        return mappedDoctors;
    }

    public async Task<OneOf<DoctorResponseDTO, NotFound>> GetDoctorByIdAsync(Guid id)
    {
        var doctor = await _doctorsRepository.GetByIdAsync(id);

        if (doctor is null)
        {
            return new NotFound();
        }

        var mappedDoctor = _mapper.Map<DoctorResponseDTO>(doctor);

        return mappedDoctor;
    }
    
    public async Task<DoctorResponseDTO> CreateDoctorAsync(DoctorCreateDTO newDoctor)
    {
        var newDoctorEntity = _mapper.Map<Doctor>(newDoctor);

        newDoctorEntity.Account = new Account()
        {
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Email = newDoctor.Email,
        };

        await _doctorsRepository.CreateAsync(newDoctorEntity);

        var doctorResult = _mapper.Map<DoctorResponseDTO>(newDoctorEntity);
            
        return doctorResult;
    }

    public async Task<OneOf<Success, NotFound>> UpdateDoctorAsync(Guid id, DoctorUpdateDTO updatedDoctor)
    {
        var doctorEntity = await _doctorsRepository.GetByIdAsync(id);

        if (doctorEntity is null)
        {
            return new NotFound();
        }

        _mapper.Map(updatedDoctor, doctorEntity);

        await _doctorsRepository.UpdateAsync(doctorEntity);

        return new Success();
    }

    public async Task<OneOf<Success, NotFound>> DeleteDoctorAsync(Guid id)
    {
        var doctorEntity = await _doctorsRepository.GetByIdAsync(id);

        if (doctorEntity is null)
        {
            return new NotFound();
        }

        await _accountsRepository.DeleteAsync(doctorEntity.Account);

        return new Success();
    }
}
