
using FluentValidation;

namespace Application.Features.Registrations.CreateRegistration;

public sealed class CreateRegistrationCommandValidator
    : AbstractValidator<CreateRegistrationCommand>
{
    public CreateRegistrationCommandValidator()
    {
        RuleFor(x => x.ParticipantName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.ContactNumber)
            .NotEmpty()
            .Matches(@"^09\d{9}$")
            .WithMessage("Contact number must be in the format 09XXXXXXXXX.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(200);

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(300);

        RuleFor(x => x.CarMakeModel)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.PlateNumber)
            .NotEmpty()
            .MaximumLength(15);

        RuleFor(x => x.RegistrationType)
            .IsInEnum();

        RuleFor(x => x.AmountPaid)
            .GreaterThanOrEqualTo(0)
            .Must((command, amount) =>
            {
                decimal fee = command.RegistrationType switch
                {
                    Domain.Enums.RegistrationType.Exhibitor => 1500m,
                    Domain.Enums.RegistrationType.Competitor => 2500m,
                    Domain.Enums.RegistrationType.VIP => 5000m,
                    _ => 0m
                };

                return amount <= fee;
            })
            .WithMessage("Amount paid cannot exceed the registration fee.");
    }
}
