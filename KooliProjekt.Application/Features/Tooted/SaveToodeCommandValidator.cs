using FluentValidation;

namespace KooliProjekt.Application.Features.Tooted
{
    public class SaveToodeCommandValidator : AbstractValidator<SaveToodeCommand>
    {
        public SaveToodeCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required")
                .MaximumLength(100);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative");
        }
    }
}