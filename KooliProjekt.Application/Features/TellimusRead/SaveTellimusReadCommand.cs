using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.TellimuseRead
{
    public class SaveTellimuseReadCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
        public int TellimusId { get; set; }
        public int ToodeId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public decimal VatRate { get; set; }
        public decimal VatAmount { get; set; }
    }
}