﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories; 
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data; 

namespace KooliProjekt.Application.Features.Kliendid
{
    public class DeleteKlientCommandHandler : IRequestHandler<DeleteKlientCommand, OperationResult>
    {
        private readonly IKlientRepository _klientRepository;

        // Konstruktor võtab vastu IKlientRepository
        public DeleteKlientCommandHandler(IKlientRepository klientRepository)
        {
            _klientRepository = klientRepository;
        }

        public async Task<OperationResult> Handle(DeleteKlientCommand request, CancellationToken cancellationToken)
        {
            // 1. Leia klient Repositoryst (kontrollimaks, kas ta on olemas)
            var klientEntity = await _klientRepository.GetByIdAsync(request.Id);

            if (klientEntity == null)
            {
                // Kui klienti ei leitud, tagasta veateade
                return OperationResult.Failure($"Klienti ID {request.Id} ei leitud, kustutamine ebaõnnestus.");
            }

            // 2. Kustuta klient Repository kaudu (Repository teeb ise SaveChangesAsync)
            await _klientRepository.DeleteAsync(klientEntity);

            // 3. Tagasta edukas tulemus
            return OperationResult.Success();
        }
    }
}