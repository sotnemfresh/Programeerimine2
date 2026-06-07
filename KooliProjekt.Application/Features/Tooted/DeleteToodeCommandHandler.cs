﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Data.Repositories; 
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tooted
{
    public class DeleteToodeCommandHandler : IRequestHandler<DeleteToodeCommand, OperationResult>
    {
        private readonly IToodeRepository _toodeRepository;

        public DeleteToodeCommandHandler(IToodeRepository toodeRepository)
        {
            _toodeRepository = toodeRepository;
        }

        public async Task<OperationResult> Handle(DeleteToodeCommand request, CancellationToken cancellationToken)
        {
            // 1. Leia Tellimus ID järgi (kasutades BaseRepository meetodit GetByIdAsync<Toode>)
            var toodeEntity = await _toodeRepository.GetByIdAsync(request.Id);

            if (toodeEntity == null)
            {
                // Kui toodet ei leitud, tagastame veateate
                return OperationResult.Failure($"Toodet ID {request.Id} ei leitud, kustutamine ebaõnnestus.");
            }

            // 2. Kustuta Toode Repository kaudu
            // Kasutame BaseRepository geneerilist DeleteAsync meetodit
            await _toodeRepository.DeleteAsync(toodeEntity);

            // 3. Tagasta edukas tulemus
            return OperationResult.Success();
        }
    }
}