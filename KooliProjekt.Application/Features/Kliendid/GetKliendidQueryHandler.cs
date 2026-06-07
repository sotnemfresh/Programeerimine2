﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Data.Repositories; 
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Kliendid
{
    public class GetKlientQueryHandler : IRequestHandler<GetKlientQuery, OperationResult<KlientDto>>
    {
        private readonly IKlientRepository _klientRepository;

        // 2. Konstruktor võtab vastu IKlientRepository (Injection)
        public GetKlientQueryHandler(IKlientRepository klientRepository)
        {
            _klientRepository = klientRepository;
        }

        public async Task<OperationResult<KlientDto>> Handle(GetKlientQuery request, CancellationToken cancellationToken)
        {
            var klientEntity = await _klientRepository.GetByIdAsync(request.Id);

            if (klientEntity == null)
            {
                // Tagasta veateade, kui Entityt ei leitud
                return OperationResult<KlientDto>.Failure("Klienti ID " + request.Id + " ei leitud.");
            }

            // 4. Teeb Mappingu Klient Entity'st KlientDto'ks (Manual Mapping)
            var klientDto = new KlientDto
            {
                Id = klientEntity.Id,
                FirstName = klientEntity.FirstName,
                LastName = klientEntity.LastName,
                Address = klientEntity.Address,
                Email = klientEntity.Email,
                Phone = klientEntity.Phone,
                Discount = klientEntity.Discount
            };

            return OperationResult<KlientDto>.Success(klientDto);
        }
    }
}