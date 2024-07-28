using InnoClinic.SharedModels.MQMessages.Offices;
using MassTransit;
using Profiles.Contracts.Pagination;
using Profiles.Domain.Interfaces;

namespace Profiles.Infrastructure.MassTransit;

public class OfficeUpdatedConsumer : IConsumer<OfficeUpdatedMessage>
{
    private readonly IDoctorsRepository _doctorsRepository;
    private readonly IReceptionistsRepository _receptionistsRepository;

    public OfficeUpdatedConsumer(IDoctorsRepository doctorsRepository, IReceptionistsRepository receptionistsRepository)
    {
        _doctorsRepository = doctorsRepository;
        _receptionistsRepository = receptionistsRepository;
    }

    public async Task Consume(ConsumeContext<OfficeUpdatedMessage> context)
    {
        var officeUpdatedMessage = context.Message;

        var doctors = await _doctorsRepository.GetAllAsync(new DoctorsQueryParameters() { OfficeId = officeUpdatedMessage.OfficeId });

        foreach (var doctor in doctors)
        {
            doctor.OfficeAddress = officeUpdatedMessage.OfficeAddress;

            await _doctorsRepository.UpdateAsync(doctor);
        }

        var receptionists = await _receptionistsRepository.GetAllAsync(new ReceptionistsQueryParameters { OfficeId = officeUpdatedMessage.OfficeId});

        foreach (var receptionist in receptionists)
        {
            receptionist.OfficeAddress = officeUpdatedMessage.OfficeAddress;

            await _receptionistsRepository.UpdateAsync(receptionist);
        }
    }
}
