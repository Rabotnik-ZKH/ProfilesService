namespace Profiles.Contracts.DTOs.Receptionist;

public class ReceptionistUpdateDTO
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string OfficeId { get; set; } = null!;
}
