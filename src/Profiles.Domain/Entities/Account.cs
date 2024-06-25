using System.ComponentModel.DataAnnotations;

namespace Profiles.Domain.Entities;

public class Account
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [EmailAddress]
    [MaxLength(256)]
    public string? Email { get; set; }

    [Phone]
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    public bool IsEmailVerified { get; set; } = false;

    public Guid? PhotoId { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Guid? UpdatedBy { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public BaseUser? User { get; set; }
}
