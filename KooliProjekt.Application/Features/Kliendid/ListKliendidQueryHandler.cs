﻿using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Dto;
namespace KooliProjekt.Application.Features.Kliendid
{
    public class ListKliendidQueryHandler : IRequestHandler<ListKliendidQuery, OperationResult<PagedResult<KlientListDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListKliendidQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<KlientListDto>>> Handle(ListKliendidQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<KlientListDto>>();

            var query = _dbContext.Kliendid
                .OrderBy(k => k.LastName)
                .Select(k => new KlientListDto
                {
                    Id = k.Id,
                    FirstName = k.FirstName,
                    LastName = k.LastName,
                    Email = k.Email,
                    Discount = k.Discount
                });

            result.Value = await query.GetPagedAsync(request.Page, request.PageSize);
            return result;
        }
    }
}