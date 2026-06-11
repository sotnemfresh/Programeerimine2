using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Tooted
{
[ExcludeFromCodeCoverage]
    public class SaveToodeCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FotoURL { get; set; }
        public decimal Price { get; set; }
        public decimal StockQuantity { get; set; }
    }
}