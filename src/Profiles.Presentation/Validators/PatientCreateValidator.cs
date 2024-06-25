using FluentValidation;
using Profiles.Contracts.DTOs.Patient;

namespace Profiles.Presentation.Validators;

public class PatientCreateValidator : AbstractValidator<PatientCreateDTO>
{
    public PatientCreateValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull().WithMessage("FirstName field have to be provided.")
            .MaximumLength(100).WithMessage("FirsName field should contain 100 or less simbols.")
            .MinimumLength(1).WithMessage("FirstName field should contain at least 1 simbol.");
        RuleFor(x => x.LastName)
            .NotNull().WithMessage("LastName field have to be provided.")
            .MaximumLength(100).WithMessage("LastName field should contain 100 or less simbols.")
            .MinimumLength(1).WithMessage("LastName field should contain at least 1 simbol.");
        RuleFor(x => x.MiddleName)
            .MinimumLength(1)
            .MaximumLength(100);
        RuleFor(x => x.PhoneNumber)
            .Matches("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$")
            .WithMessage("Wrong phone format.")
            .NotEmpty();
        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now)
            .GreaterThan(new DateTime(1900, 01, 01));
    }
}
