using Profiles.Contracts.DTOs.OuterServicesModels;

namespace Profiles.Contracts.DTOs.Doctor;

public class DoctorResponseDTO()
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int Experience { get; set; }

    public int Status { get; set; } 

    public string SpecializationName { get; set; }

    public string OfficeAddress { get; set; }
}

