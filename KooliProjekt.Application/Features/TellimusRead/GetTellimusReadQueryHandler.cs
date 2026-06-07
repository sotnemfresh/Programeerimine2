﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.TellimuseRead
{
    public class GetTellimuseReadQueryHandler : IRequestHandler<GetTellimuseReadQuery, OperationResult<TellimuseRidaDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetTellimuseReadQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<TellimuseRidaDto>> Handle(GetTellimuseReadQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Id <= 0)
            {
                return new OperationResult<TellimuseRidaDto> { Value = null };
            }

            var result = new OperationResult<TellimuseRidaDto>();

            var rida = await _dbContext.TellimusedRida
                .Include(r => r.Toode)
                .Where(r => r.Id == request.Id)
                .Select(r => new TellimuseRidaDto
                {
                    Id = r.Id,
                    Quantity = r.Quantity,
                    UnitPrice = r.UnitPrice,
                    LineTotal = r.LineTotal,
                    TellimusId = r.TellimusId,
                    Toode = new ToodeListDto
                    {
                        Id = r.Toode.Id,
                        Name = r.Toode.Name,
                        FotoURL = r.Toode.FotoURL,
                        Price = r.Toode.Price,
                        StockQuantity = r.Toode.StockQuantity
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            result.Value = rida;
            return result;
        }
    }
}