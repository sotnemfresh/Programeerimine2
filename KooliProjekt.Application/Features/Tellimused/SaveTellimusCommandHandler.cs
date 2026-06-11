using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tellimused
{
    public class SaveTellimusCommandHandler : IRequestHandler<SaveTellimusCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveTellimusCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new System.ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveTellimusCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new System.ArgumentNullException(nameof(request));

            var result = new OperationResult();
            if (request.Id < 0)
            {
                result.AddError("Request ID cannot be negative");
                return result;
            }

            Tellimus tellimus;

            if (request.Id == 0)
            {
                tellimus = new Tellimus();
                await _dbContext.Tellimused.AddAsync(tellimus, cancellationToken);
            }
            else
            {
                tellimus = await _dbContext.Tellimused
                    .Include(t => t.TellimuseRead)
                    .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                if (tellimus == null)
                {
                    result.AddError("Cannot find order with ID " + request.Id);
                    return result;
                }
            }

            tellimus.OrderDate = request.OrderDate;
            tellimus.Status = request.Status;
            tellimus.KlientId = request.KlientId;

            if (request.TellimuseRead != null)
            {
                foreach (var ridaDto in request.TellimuseRead)
                {
                    var rida = tellimus.TellimuseRead.FirstOrDefault(tr => tr.Id == ridaDto.Id) ?? new TellimuseRida();
                    if (rida.Id == 0) tellimus.TellimuseRead.Add(rida);

                    rida.ToodeId = ridaDto.ToodeId;
                    rida.Quantity = ridaDto.Quantity;
                    rida.UnitPrice = ridaDto.UnitPrice;
                    rida.LineTotal = ridaDto.LineTotal;
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}