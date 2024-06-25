namespace Profiles.Contracts.Pagination;

public class DoctorsQueryParameters
{
    public Guid? SpecializationId { get; set; }

    public string? SearchFullName {  get; set; }
}
