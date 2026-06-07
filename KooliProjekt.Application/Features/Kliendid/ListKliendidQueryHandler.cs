﻿using System;
using KooliProjekt.Application.Data;
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
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<KlientListDto>>> Handle(ListKliendidQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Page <= 0) throw new ArgumentException("Page must be greater than 0", nameof(request.Page));
            if (request.PageSize <= 0) throw new ArgumentException("PageSize must be greater than 0", nameof(request.PageSize));
            if (request.PageSize > 100) throw new ArgumentException("PageSize cannot be greater than 100", nameof(request.PageSize));

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