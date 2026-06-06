using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Features.Tellimused
{
    public class SaveTellimusCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public int KlientId { get; set; }

        public IList<SaveTellimuseRidaDto> TellimuseRead { get; set; } = new List<SaveTellimuseRidaDto>();
    }

    public class SaveTellimuseRidaDto
    {
        public int Id { get; set; }
        public int ToodeId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public decimal VatRate { get; set; }
        public decimal VatAmount { get; set; }
    }
}