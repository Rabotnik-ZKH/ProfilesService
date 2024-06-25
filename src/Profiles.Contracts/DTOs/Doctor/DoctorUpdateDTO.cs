namespace Profiles.Contracts.DTOs.Doctor;

public class DoctorUpdateDTO
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public Guid SpecializationId { get; set; }

    public string OfficeId { get; set; } = null!;

    public DateTime CareerStartYear { get; set; }

    public int Status { get; set; }
}
