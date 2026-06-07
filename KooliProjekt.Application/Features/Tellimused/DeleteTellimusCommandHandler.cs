using System;
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
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteTellimusCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();
            if (request.Id <= 0) return result;

            var tellimus = await _dbContext.Tellimused
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (tellimus == null) return result;

            var read = await _dbContext.TellimusedRida
                .Where(r => r.TellimusId == request.Id)
                .ToListAsync(cancellationToken);

            if (read.Any())
            {
                _dbContext.TellimusedRida.RemoveRange(read);
            }

            _dbContext.Tellimused.Remove(tellimus);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}