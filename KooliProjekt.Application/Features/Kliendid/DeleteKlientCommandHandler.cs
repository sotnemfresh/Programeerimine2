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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteKlientCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            // 1. Delete all TellimusedRida (order line items) for this client's orders
            await _dbContext.TellimusedRida
                .Where(tr => tr.Tellimus.KlientId == request.Id)
                .ExecuteDeleteAsync();

            // 2. Delete all Arved (invoices) for this client
            await _dbContext.Arved
                .Where(a => a.KlientId == request.Id)
                .ExecuteDeleteAsync();

            // 3. Delete all Tellimused (orders) for this client
            await _dbContext.Tellimused
                .Where(t => t.KlientId == request.Id)
                .ExecuteDeleteAsync();

            // 4. Finally, delete the Klient
            await _dbContext.Kliendid
                .Where(k => k.Id == request.Id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}