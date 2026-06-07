using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.TellimuseRead
{
    public class ListTellimuseReadQueryHandler : IRequestHandler<ListTellimuseReadQuery, OperationResult<PagedResult<TellimuseRidaListDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListTellimuseReadQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<TellimuseRidaListDto>>> Handle(ListTellimuseReadQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Page <= 0) throw new ArgumentException("Page must be greater than 0", nameof(request.Page));
            if (request.PageSize <= 0) throw new ArgumentException("PageSize must be greater than 0", nameof(request.PageSize));
            if (request.PageSize > 100) throw new ArgumentException("PageSize cannot be greater than 100", nameof(request.PageSize));

            var result = new OperationResult<PagedResult<TellimuseRidaListDto>>();

            var query = _dbContext.TellimusedRida
                .Include(r => r.Toode)
                .OrderBy(r => r.Id)
                .Select(r => new TellimuseRidaListDto
                {
                    Id = r.Id,
                    Quantity = r.Quantity,
                    ToodeName = r.Toode.Name,
                    LineTotal = r.LineTotal,
                    TellimusId = r.TellimusId
                })
                .AsQueryable();

            result.Value = await query.GetPagedAsync(request.Page, request.PageSize);
            return result;
        }
    }
}