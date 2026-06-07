﻿using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data; 
using KooliProjekt.Application.Data.Repositories; 
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Kliendid
{
    public class SaveKlientCommandHandler : IRequestHandler<SaveKlientCommand, OperationResult>
    {
        private readonly IKlientRepository _klientRepository;

        public SaveKlientCommandHandler(IKlientRepository klientRepository)
        {
            _klientRepository = klientRepository;
        }

        public async Task<OperationResult> Handle(SaveKlientCommand request, CancellationToken cancellationToken)
        {
            Klient klientEntity;

            if (request.Id == 0)
            {

                klientEntity = new Klient();
            }
            else
            {
                // Olemasoleva kliendi leidmine (kasutades Repository meetodit)
                klientEntity = await _klientRepository.GetByIdAsync(request.Id);

                if (klientEntity == null)
                {
                    // Kui klienti ei leitud, tagasta veateade
                    return OperationResult.Failure($"Klienti ID {request.Id} ei leitud.");
                }
            }

            // Mapime Command'i väärtused Entity'sse
            klientEntity.FirstName = request.FirstName;
            klientEntity.LastName = request.LastName;
            klientEntity.Address = request.Address;
            klientEntity.Email = request.Email;
            klientEntity.Phone = request.Phone;
            klientEntity.Discount = request.Discount;

            // Kasutame Repository.SaveAsync(), mis tegeleb Add/Update/SaveChangesAsync()
            await _klientRepository.SaveAsync(klientEntity);

            // Tagastame eduka tulemuse
            return OperationResult.Success();
        }
    }
}