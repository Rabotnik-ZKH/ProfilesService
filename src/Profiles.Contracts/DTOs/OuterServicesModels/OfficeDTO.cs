namespace Profiles.Contracts.DTOs.OuterServicesModels;

public class OfficeDTO
{
    public string OfficeId { get; set; } = null!;

    public string? PhotoId { get; set; } 

    public string City { get; set; }

    public string Street { get; set; }

    public string OfficeNumber { get; set; }

    public string RegistryPhoneNumber { get; set; } = null!;

    public Status IsActive { get; set; }
}
