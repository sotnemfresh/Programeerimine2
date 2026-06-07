using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.TellimuseRead
{
    public class DeleteTellimuseReadCommandHandler : IRequestHandler<DeleteTellimuseReadCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteTellimuseReadCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteTellimuseReadCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();
            if (request.Id <= 0) return result;

            var rida = await _dbContext.TellimusedRida
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (rida == null) return result;

            _dbContext.TellimusedRida.Remove(rida);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}