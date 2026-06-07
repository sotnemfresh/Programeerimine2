﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data; 
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore; 

namespace KooliProjekt.Application.Features.Tellimused
{
    public class DeleteTellimusCommandHandler : IRequestHandler<DeleteTellimusCommand, OperationResult>
    {
        private readonly ITellimusRepository _tellimusRepository;

        public DeleteTellimusCommandHandler(ITellimusRepository tellimusRepository)
        {
            _tellimusRepository = tellimusRepository;
        }

        public async Task<OperationResult> Handle(DeleteTellimusCommand request, CancellationToken cancellationToken)
        {
            // 1. Leia Tellimus ID järgi
            // Kasutame BaseRepository meetodit GetByIdAsync<Tellimus>
            var tellimusEntity = await _tellimusRepository.GetByIdAsync(request.Id);

            if (tellimusEntity == null)
            {
                // Kui tellimust ei leitud, tagastame veateate
                return OperationResult.Failure($"Tellimust ID {request.Id} ei leitud, kustutamine ebaõnnestus.");
            }

            // 2. Kustuta Tellimus Repository kaudu
            // Repository tegeleb SaveChangesAsync()
            await _tellimusRepository.DeleteAsync(tellimusEntity);

            // 3. Tagasta edukas tulemus
            return OperationResult.Success();
        }
    }
}