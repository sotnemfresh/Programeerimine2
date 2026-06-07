using FluentValidation;

namespace KooliProjekt.Application.Features.Arved
{
    public class SaveArveCommandValidator : AbstractValidator<SaveArveCommand>
    {
        public SaveArveCommandValidator()
        {
            // Kontrollime, et arve number poleks tühi ja poleks liiga pikk
            RuleFor(x => x.InvoiceNumber)
                .NotEmpty().WithMessage("Invoice number is required")
                .MaximumLength(50).WithMessage("Invoice number cannot exceed 50 characters");

            //kontroll TellimusId jaoks
            RuleFor(x => x.TellimusId)
                .GreaterThan(0).WithMessage("A valid Order ID is required");
        }
    }
}