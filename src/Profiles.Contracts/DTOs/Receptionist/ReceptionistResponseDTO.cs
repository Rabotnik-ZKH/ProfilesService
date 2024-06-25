using Profiles.Contracts.DTOs.OuterServicesModels;

namespace Profiles.Contracts.DTOs.Receptionist;

public class ReceptionistResponseDTO
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public OfficeDTO? Office { get; set; }
}
