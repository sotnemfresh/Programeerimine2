using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tooted
{
    public class GetToodeQueryHandler : IRequestHandler<GetToodeQuery, OperationResult<ToodeDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetToodeQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<ToodeDto>> Handle(GetToodeQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<ToodeDto>();

            var toode = await _dbContext.Tooted
                .Where(t => t.Id == request.Id)
                .Select(t => new ToodeDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    FotoURL = t.FotoURL,
                    Price = t.Price,
                    StockQuantity = t.StockQuantity
                })
                .FirstOrDefaultAsync();

            result.Value = toode;
            return result;
        }
    }
}