﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Data.Repositories; 
using MediatR;

namespace KooliProjekt.Application.Features.Tooted
{
    public class GetToodeQueryHandler : IRequestHandler<GetToodeQuery, OperationResult<ToodeDto>>
    {
        private readonly IToodeRepository _toodeRepository;

        public GetToodeQueryHandler(IToodeRepository toodeRepository)
        {
            _toodeRepository = toodeRepository;
        }

        public async Task<OperationResult<ToodeDto>> Handle(GetToodeQuery request, CancellationToken cancellationToken)
        {
            // Kutsuge välja repositori meetod, mis teeb kogu töö
            var toodeDto = await _toodeRepository.GetToodeDetailsDtoAsync(request.Id);

            if (toodeDto == null)
            {
                // Kui toodet ei leitud, tagastame vea
                return OperationResult<ToodeDto>.Failure($"Toodet ID {request.Id} ei leitud.");
            }

            // Tagasta edukas tulemus
            return OperationResult<ToodeDto>.Success(toodeDto);
        }
    }
}