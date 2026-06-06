using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tooted
{
    public class DeleteToodeCommandHandler : IRequestHandler<DeleteToodeCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteToodeCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteToodeCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext.Tooted
                .Where(t => t.Id == request.Id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}