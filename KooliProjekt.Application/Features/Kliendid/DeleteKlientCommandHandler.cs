using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Kliendid
{
    public class DeleteKlientCommandHandler : IRequestHandler<DeleteKlientCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteKlientCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteKlientCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.Id <= 0)
            {
                return result;
            }

            var klient = await _dbContext.Kliendid
                .FirstOrDefaultAsync(k => k.Id == request.Id, cancellationToken);

            if (klient == null)
            {
                return result;
            }

            _dbContext.Kliendid.Remove(klient);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}