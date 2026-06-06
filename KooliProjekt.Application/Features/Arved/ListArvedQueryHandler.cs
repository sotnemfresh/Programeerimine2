using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Dto;

namespace KooliProjekt.Application.Features.Arved
{
    public class ListArvedQueryHandler : IRequestHandler<ListArvedQuery, OperationResult<PagedResult<ArveListDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListArvedQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<ArveListDto>>> Handle(ListArvedQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<ArveListDto>>();

            var query = _dbContext.Arved
                .Include(a => a.Klient)
                .OrderBy(a => a.InvoiceDate)
                .Select(a => new ArveListDto
                {
                    Id = a.Id,
                    InvoiceNumber = a.InvoiceNumber,
                    InvoiceDate = a.InvoiceDate,
                    GrandTotal = a.GrandTotal,
                    Status = a.Status,
                    Klient = new KlientListDto
                    {
                        Id = a.Klient.Id,
                        FirstName = a.Klient.FirstName,
                        LastName = a.Klient.LastName,
                        Email = a.Klient.Email,
                        Discount = a.Klient.Discount
                    }
                })
                .AsQueryable();

            result.Value = await query.GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}