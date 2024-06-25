namespace Profiles.Contracts.DTOs.OuterServicesModels;

public class OfficeDTO
{
    public string OfficeId { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? PhotoId { get; set; }

    public string RegistryPhoneNumber { get; set; } = null!;

    public bool IsActive { get; set; }
}
