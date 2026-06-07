﻿using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Data.Repositories; 
using MediatR;

namespace KooliProjekt.Application.Features.Tooted
{
    public class SaveToodeCommandHandler : IRequestHandler<SaveToodeCommand, OperationResult>
    {
        private readonly IToodeRepository _toodeRepository;

        public SaveToodeCommandHandler(IToodeRepository toodeRepository)
        {
            _toodeRepository = toodeRepository;
        }

        public async Task<OperationResult> Handle(SaveToodeCommand request, CancellationToken cancellationToken)
        {
            Toode toode;

            if (request.Id == 0)
            {
                // Uus toode
                toode = new Toode();
            }
            else
            {
                // Olemasolev toode, laeme sisse ID järgi (kasutades BaseRepository meetodit)
                toode = await _toodeRepository.GetByIdAsync(request.Id);

                if (toode == null)
                {
                    return OperationResult.Failure($"Toodet ID {request.Id} ei leitud.");
                }
            }

            // Mapime Commandi andmed Entitysse
            toode.Name = request.Name;
            toode.Description = request.Description;
            toode.FotoURL = request.FotoURL;
            toode.Price = request.Price;
            toode.StockQuantity = request.StockQuantity;

            // Kasutame BaseRepository geneerilist SaveAsync meetodit
            await _toodeRepository.SaveAsync(toode);

            return OperationResult.Success();
        }
    }
}