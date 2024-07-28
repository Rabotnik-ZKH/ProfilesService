using AutoMapper;
using InnoClinic.SharedModels.MQMessages.Profiles;
using MassTransit;
using MassTransit.Testing;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using OneOf.Types;
using Profiles.Contracts.DTOs.OuterServicesModels;
using Profiles.Contracts.DTOs.Receptionist;
using Profiles.Contracts.Pagination;
using Profiles.Domain.Entities;
using Profiles.Domain.Interfaces;
using Profiles.Infrastructure.Repositories;
using Profiles.Services.Abstractions;
using System.Text;

namespace Profiles.Services.Services;

public class ReceptionistsService : IReceptionistsService
{
    private readonly IAccountsRepository _accountsRepository;
    private readonly IReceptionistsRepository _receptionistsRepository;
    private readonly IHttpRepository<OfficeDTO> _httpRepository;
    private readonly IPublishEndpoint _messagePublisher;
    private readonly IMapper _mapper;

    public ReceptionistsService(IReceptionistsRepository receptionistsRepository, 
        IMapper mapper, IHttpRepository<OfficeDTO> httpRepository, 
        IAccountsRepository accountsRepository,
        IPublishEndpoint messagePublisher)
    {
        _receptionistsRepository = receptionistsRepository;
        _mapper = mapper;
        _httpRepository = httpRepository;
        _accountsRepository = accountsRepository;
        _messagePublisher = messagePublisher;
    }

    public async Task<OneOf<List<ReceptionistResponseDTO>, NotFound>> GetAllReceptionistsAsync(ReceptionistsQueryParameters parameters)
    {
        var receptionists = await _receptionistsRepository.GetAllAsync(parameters);

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

    public async Task<Guid> CreateReceptionistAsync(ReceptionistCreateDTO newReceptionist)
    {
        var newReceptionistEntity = _mapper.Map<Receptionist>(newReceptionist);

        newReceptionistEntity.Account = new Account()
        {
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Email = newReceptionist.Email,
        };
        
        await _receptionistsRepository.CreateAsync(newReceptionistEntity);

        var receptionistPassword = GeneratePassword(6);

        await _messagePublisher.Publish<WorkerProfileCreatedMessage>(new()
        {
            Email = newReceptionist.Email,
            Password = receptionistPassword,
            Role = "Receptionist"
        });

        return newReceptionistEntity.Id;
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


    private string GeneratePassword(int length)
    {
        const string symbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        StringBuilder res = new StringBuilder();
        Random rnd = new Random();

        while (0 < length--)
        {
            res.Append(symbols[rnd.Next(symbols.Length)]);
        }

        return res.ToString();
    }
}
