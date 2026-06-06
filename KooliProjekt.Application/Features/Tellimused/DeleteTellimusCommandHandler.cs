using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tellimused
{
    public class DeleteTellimusCommandHandler : IRequestHandler<DeleteTellimusCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteTellimusCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteTellimusCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext.Tellimused
                .Where(t => t.Id == request.Id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}