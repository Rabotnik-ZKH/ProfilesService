using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using OneOf.Types;
using Profiles.Contracts.DTOs.OuterServicesModels;
using Profiles.Contracts.DTOs.Receptionist;
using Profiles.Domain.Entities;
using Profiles.Domain.Interfaces;
using Profiles.Services.Abstractions;

namespace Profiles.Services.Services;

public class ReceptionistsService : IReceptionistsService
{
    private readonly IAccountsRepository _accountsRepository;
    private readonly IReceptionistsRepository _receptionistsRepository;
    private readonly IHttpRepository<OfficeDTO> _httpRepository;
    private readonly IMapper _mapper;

    public ReceptionistsService(IReceptionistsRepository receptionistsRepository, 
        IMapper mapper, IHttpRepository<OfficeDTO> httpRepository, IAccountsRepository accountsRepository)
    {
        _receptionistsRepository = receptionistsRepository;
        _mapper = mapper;
        _httpRepository = httpRepository;
        _accountsRepository = accountsRepository;
    }

    public async Task<OneOf<List<ReceptionistResponseDTO>, NotFound>> GetAllReceptionistsAsync()
    {
        var receptionists = await _receptionistsRepository.GetAllAsync();

        if (receptionists.IsNullOrEmpty())
        {
            return new NotFound();
        }

        var mappedReceptionists = _mapper.Map<List<ReceptionistResponseDTO>>(receptionists);

        IEnumerable<string> officesIds = receptionists.Select(x => x.OfficeId).Distinct().ToList();

        var offices = await _httpRepository.GetCollection(officesIds);

        foreach (var receptionist in mappedReceptionists)
        {
            receptionist.Office = offices.First(x => x.OfficeId.Equals(receptionist.Office.OfficeId));
        }

        return mappedReceptionists;
    }

    public async Task<OneOf<ReceptionistResponseDTO, NotFound>> GetReceptionistByIdAsync(Guid id)
    {
        var receptionist = await _receptionistsRepository.GetByIdAsync(id);

        if (receptionist is null)
        {
            return new NotFound();
        }

        var mappedReceptionist = _mapper.Map<ReceptionistResponseDTO>(receptionist);

        mappedReceptionist.Office = 
            await _httpRepository.GetOneAsync("https://localhost:7255/api/offices", receptionist.OfficeId!);

        return mappedReceptionist;
    }

    public async Task<ReceptionistResponseDTO> CreateReceptionistAsync(ReceptionistCreateDTO newReceptionist)
    {
        var newReceptionistEntity = _mapper.Map<Receptionist>(newReceptionist);

        newReceptionistEntity.Account = new Account()
        {
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Email = newReceptionist.Email,
        };
        
        await _receptionistsRepository.CreateAsync(newReceptionistEntity);

        var receptionistResult = _mapper.Map<ReceptionistResponseDTO>(newReceptionistEntity);

        receptionistResult.Office =  
            await _httpRepository.GetOneAsync("https://localhost:7255/api/offices", newReceptionistEntity.OfficeId!);

        return receptionistResult;
    }

    public async Task<OneOf<Success, NotFound>> UpdateReceptionistAsync(Guid id, ReceptionistUpdateDTO updatedReceptionist)
    {
        var receptionistEntity = await _receptionistsRepository.GetByIdAsync(id);

        if (receptionistEntity is null)
        {
            return new NotFound();
        }

        _mapper.Map(updatedReceptionist, receptionistEntity);

        await _receptionistsRepository.UpdateAsync(receptionistEntity);

        return new Success();
    }

    public async Task<OneOf<Success, NotFound>> DeleteReceptionistAsync(Guid id)
    {
        var receptionistEntity = await _receptionistsRepository.GetByIdAsync(id);

        if (receptionistEntity is null)
        {
            return new NotFound();
        }

        await _accountsRepository.DeleteAsync(receptionistEntity.Account);

        return new Success();
    }
}
