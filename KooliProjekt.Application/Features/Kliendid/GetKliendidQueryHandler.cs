using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Kliendid
{
    public class GetKlientQueryHandler : IRequestHandler<GetKlientQuery, OperationResult<KlientDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetKlientQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<KlientDto>> Handle(GetKlientQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<KlientDto>();

            var klient = await _dbContext.Kliendid
                .Where(k => k.Id == request.Id)
                .Select(k => new KlientDto
                {
                    Id = k.Id,
                    FirstName = k.FirstName,
                    LastName = k.LastName,
                    Address = k.Address,
                    Email = k.Email,
                    Phone = k.Phone,
                    Discount = k.Discount
                })
                .FirstOrDefaultAsync();

            result.Value = klient;
            return result;
        }
    }
}