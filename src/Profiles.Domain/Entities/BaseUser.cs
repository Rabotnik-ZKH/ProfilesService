using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profiles.Domain.Entities;

public class BaseUser
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [MaxLength(100)]
    public string? MiddleName { get; set; }

    public DateTime? DateOfBirth { get; set; } = null;

    [MaxLength(24)]
    public string? OfficeId { get; set; }

    [Required]
    [ForeignKey(nameof(Account))]
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
}
