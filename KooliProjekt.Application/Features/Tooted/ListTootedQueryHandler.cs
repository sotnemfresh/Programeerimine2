using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Dto;
namespace KooliProjekt.Application.Features.Tooted
{
    public class ListTootedQueryHandler : IRequestHandler<ListTootedQuery, OperationResult<PagedResult<ToodeListDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListTootedQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<ToodeListDto>>> Handle(ListTootedQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<ToodeListDto>>();

            var query = _dbContext.Tooted
                .OrderBy(t => t.Name)
                .Select(t => new ToodeListDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    FotoURL = t.FotoURL,
                    Price = t.Price,
                    StockQuantity = t.StockQuantity
                });

            result.Value = await query.GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}