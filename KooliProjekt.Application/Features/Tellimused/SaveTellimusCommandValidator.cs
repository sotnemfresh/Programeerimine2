using FluentValidation;

namespace KooliProjekt.Application.Features.Tellimused
{
    public class SaveTellimusCommandValidator : AbstractValidator<SaveTellimusCommand>
    {
        public SaveTellimusCommandValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .MaximumLength(20).WithMessage("Status is too long");
        }
    }
}