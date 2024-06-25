using FluentValidation;
using Profiles.Contracts.DTOs.Patient;

namespace Profiles.Presentation.Validators;

public class PatientUpdateValidator : AbstractValidator<PatientUpdateDTO>
{
    public PatientUpdateValidator()
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
            .MinimumLength(1).WithMessage("MiddleName field should contain at least 1 simbol.")
            .MaximumLength(100).WithMessage("MiddleName field should contain 100 or less simbols.");
        RuleFor(x => x.PhoneNumber)
            .Matches("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$")
            .WithMessage("Wrong phone format.")
            .NotEmpty().WithMessage("Phone number field can not be empty.");
        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now).WithMessage("Wrong date of birth.")
            .GreaterThan(new DateTime(1900, 01, 01)).WithMessage("Wrong date of birth.");
    }
}
