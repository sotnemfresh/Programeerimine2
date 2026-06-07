using System;
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
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteArveCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();
            if (request.Id <= 0) return result;

            var arve = await _dbContext.Arved
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (arve == null) return result;

            _dbContext.Arved.Remove(arve);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}