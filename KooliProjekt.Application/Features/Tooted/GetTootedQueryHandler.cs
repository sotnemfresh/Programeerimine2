using System;
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
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<ToodeDto>> Handle(GetToodeQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return new OperationResult<ToodeDto> { Value = null };
            }

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
                .FirstOrDefaultAsync(cancellationToken);

            result.Value = toode;
            return result;
        }
    }
}