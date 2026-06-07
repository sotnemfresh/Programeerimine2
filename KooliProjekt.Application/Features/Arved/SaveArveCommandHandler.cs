﻿using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Data.Repositories; 
using MediatR;

namespace KooliProjekt.Application.Features.Arved
{
    public class SaveArveCommandHandler : IRequestHandler<SaveArveCommand, OperationResult>
    {
        private readonly IArveRepository _arveRepository;
        // Kasulik on ka Klientide ja Tellimuste olemasolu kontrollimiseks
        private readonly IKlientRepository _klientRepository;
        private readonly ITellimusRepository _tellimusRepository;

        public SaveArveCommandHandler(IArveRepository arveRepository, IKlientRepository klientRepository, ITellimusRepository tellimusRepository)
        {
            _arveRepository = arveRepository;
            _klientRepository = klientRepository;
            _tellimusRepository = tellimusRepository;
        }

        public async Task<OperationResult> Handle(SaveArveCommand request, CancellationToken cancellationToken)
        {
            // Valideerimine (Valikuline, aga hea tava)
            if (await _klientRepository.GetByIdAsync(request.KlientId) == null)
            {
                return OperationResult.Failure($"Klienti ID {request.KlientId} ei leitud.");
            }
            if (await _tellimusRepository.GetByIdAsync(request.TellimusId) == null)
            {
                return OperationResult.Failure($"Tellimust ID {request.TellimusId} ei leitud.");
            }

            Arve arve;
            if (request.Id == 0)
            {
                arve = new Arve();
            }
            else
            {
                // Lae olemasolev Entity
                arve = await _arveRepository.GetByIdAsync(request.Id);
                if (arve == null)
                {
                    return OperationResult.Failure($"Arvet ID {request.Id} ei leitud.");
                }
            }

            // Mapime Commandi andmed Entitysse
            arve.InvoiceNumber = request.InvoiceNumber;
            arve.InvoiceDate = request.InvoiceDate;
            arve.DueDate = request.DueDate;
            arve.Status = request.Status;
            arve.SubTotal = request.SubTotal;
            arve.ShippingTotal = request.ShippingTotal;
            arve.Discount = request.Discount;
            arve.GrandTotal = request.GrandTotal;
            arve.KlientId = request.KlientId;
            arve.TellimusId = request.TellimusId;

            // Salvesta (kasutades Base meetodit)
            await _arveRepository.SaveAsync(arve);

            return OperationResult.Success();
        }
    }
}