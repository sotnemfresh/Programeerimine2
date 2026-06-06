using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Arved
{
    public class DeleteArveCommandHandler : IRequestHandler<DeleteArveCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteArveCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteArveCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext.Arved
                .Where(a => a.Id == request.Id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}