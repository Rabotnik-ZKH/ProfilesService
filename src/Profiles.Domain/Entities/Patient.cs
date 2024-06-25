namespace Profiles.Domain.Entities;

public class Patient : BaseUser
{
    public bool IsLinkedToAccount { get; set; } = false;
}
