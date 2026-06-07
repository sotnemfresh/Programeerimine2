﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Data.Repositories; 
using MediatR;

namespace KooliProjekt.Application.Features.Tellimused
{
    public class GetTellimusQueryHandler : IRequestHandler<GetTellimusQuery, OperationResult<TellimusDto>>
    {
        private readonly ITellimusRepository _tellimusRepository;

        public GetTellimusQueryHandler(ITellimusRepository tellimusRepository)
        {
            _tellimusRepository = tellimusRepository;
        }

        public async Task<OperationResult<TellimusDto>> Handle(GetTellimusQuery request, CancellationToken cancellationToken)
        {
            // Kutsuge välja uus repositori meetod, mis teeb kogu töö
            var tellimusDto = await _tellimusRepository.GetTellimusDetailsDtoAsync(request.Id);

            if (tellimusDto == null)
            {
                // Kasuta OperationResult<T> Failure meetodit
                return OperationResult<TellimusDto>.Failure($"Tellimust ID {request.Id} ei leitud.");
            }

            // Tagasta edukas tulemus
            return OperationResult<TellimusDto>.Success(tellimusDto);
        }
    }
}