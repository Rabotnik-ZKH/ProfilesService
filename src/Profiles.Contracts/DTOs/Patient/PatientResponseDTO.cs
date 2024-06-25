namespace Profiles.Contracts.DTOs.Patient;

public class PatientResponseDTO
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null;
    
    public string? MiddleName { get; set; }
    
    public string PhoneNumber { get; set; } = null!;

    public DateTime DateOfBirth { get; set;}
}
