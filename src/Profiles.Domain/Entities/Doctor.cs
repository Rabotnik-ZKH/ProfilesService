using System.ComponentModel.DataAnnotations;
namespace Profiles.Domain.Entities;

public class Doctor : BaseUser
{
    [Required]
    public DateTime CareerStartYear { get; set; }

    [Required]
    public Status Status { get; set; }

    [Required]
    public int SpecializationId { get; set; }
}
