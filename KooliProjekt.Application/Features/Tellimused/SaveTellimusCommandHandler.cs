﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data.Repositories;

namespace KooliProjekt.Application.Features.Tellimused
{
    public class SaveTellimusCommandHandler : IRequestHandler<SaveTellimusCommand, OperationResult>
    {
        private readonly ITellimusRepository _tellimusRepository;

        // Lisame IKlientRepository, et kontrollida KlientId olemasolu
        private readonly IKlientRepository _klientRepository;

        public SaveTellimusCommandHandler(ITellimusRepository tellimusRepository, IKlientRepository klientRepository)
        {
            _tellimusRepository = tellimusRepository;
            _klientRepository = klientRepository;
        }

        public async Task<OperationResult> Handle(SaveTellimusCommand request, CancellationToken cancellationToken)
        {
            // 1. Kontrolli KlientId olemasolu
            if (request.KlientId != 0 && await _klientRepository.GetByIdAsync(request.KlientId) == null)
            {
                return OperationResult.Failure($"Klienti ID {request.KlientId} ei leitud.");
            }

            Tellimus tellimus;

            if (request.Id == 0)
            {
                // Uus tellimus
                tellimus = new Tellimus();
            }
            else
            {
                // Olemasolev tellimus: laeme sisse READ, et neid saaks muuta
                tellimus = await _tellimusRepository.GetByIdAsync(request.Id);

                if (tellimus == null)
                {
                    return OperationResult.Failure($"Tellimust ID {request.Id} ei leitud.");
                }

                // Eraldi laeme Read sisse, kui need pole veel laetud (BaseRepository.GetByIdAsync ei tee Include'i)
                await _tellimusRepository.LoadTellimuseReadAsync(tellimus);
            }

            // 2. Kutsuge välja spetsiaalne repositori meetod, mis teeb kogu töö (SaveTellimusDetailsAsync)
            await _tellimusRepository.SaveTellimusDetailsAsync(tellimus, request);

            // 3. Tagasta edukas tulemus
            return OperationResult.Success();
        }
    }
}