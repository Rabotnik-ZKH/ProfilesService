using System.ComponentModel.DataAnnotations;

namespace Profiles.Contracts.DTOs.Receptionist;

public class ReceptionistCreateDTO
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    [EmailAddress]
    public string Email { get; set; } = null!;

    public string OfficeId { get; set; } = null!;
}
