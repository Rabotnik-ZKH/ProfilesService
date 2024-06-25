using FluentValidation;
using Profiles.Contracts.DTOs.Receptionist;

namespace Profiles.Presentation.Validators;

public class ReceptionistCreateValidator : AbstractValidator<ReceptionistCreateDTO>
{
    public ReceptionistCreateValidator()
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
        RuleFor(x => x.Email)
            .NotNull().WithMessage("Email field have to be provided.")
            .EmailAddress().WithMessage("Email field is not correct.");
        RuleFor(x => x.OfficeId)
            .Length(24).WithMessage("Wrong office id length.");
    }
}
