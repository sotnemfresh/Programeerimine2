using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Dto;

namespace KooliProjekt.Application.Features.Tellimused
{
    public class ListTellimusedQueryHandler : IRequestHandler<ListTellimusedQuery, OperationResult<PagedResult<TellimusListDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListTellimusedQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

public async Task<OperationResult<PagedResult<TellimusListDto>>> Handle(ListTellimusedQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<TellimusListDto>>();

            var query = _dbContext.Tellimused
                .Include(t => t.Klient)
                .Include(t => t.TellimuseRead)
                .OrderByDescending(t => t.OrderDate)
                .Select(t => new TellimusListDto
                {
                    Id = t.Id,
                    OrderDate = t.OrderDate,
                    Status = t.Status,
                    KlientFirstName = t.Klient.FirstName,
                    KlientLastName = t.Klient.LastName,
                    RidadeArv = t.TellimuseRead.Count
                })
                .AsQueryable();

            result.Value = await query.GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}