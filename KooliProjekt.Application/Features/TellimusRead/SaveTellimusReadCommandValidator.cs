using FluentValidation;

namespace KooliProjekt.Application.Features.TellimuseRead
{
    public class SaveTellimuseReadCommandValidator : AbstractValidator<SaveTellimuseReadCommand>
    {
        public SaveTellimuseReadCommandValidator()
        {
            RuleFor(x => x.TellimusId).GreaterThan(0).WithMessage("Tellimus ID is required");
            RuleFor(x => x.ToodeId).GreaterThan(0).WithMessage("Toode ID is required");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }
}