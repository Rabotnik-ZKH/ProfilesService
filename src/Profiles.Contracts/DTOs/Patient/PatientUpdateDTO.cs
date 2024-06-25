using System.ComponentModel.DataAnnotations;

namespace Profiles.Contracts.DTOs.Patient;

public class PatientUpdateDTO
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    [Phone]
    public string PhoneNumber { get; set; } = null!;    

    public DateTime DateOfBirth { get; set; }
}
