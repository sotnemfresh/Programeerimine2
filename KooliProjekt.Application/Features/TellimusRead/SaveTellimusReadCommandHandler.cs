using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.TellimuseRead
{
    public class SaveTellimuseReadCommandHandler : IRequestHandler<SaveTellimuseReadCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveTellimuseReadCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new System.ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveTellimuseReadCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new System.ArgumentNullException(nameof(request));
            
            var result = new OperationResult();
            if (request.Id < 0)
            {
                result.AddError("Request ID cannot be negative");
                return result;
            }

            TellimuseRida rida;

            if (request.Id == 0)
            {
                rida = new TellimuseRida();
                await _dbContext.TellimusedRida.AddAsync(rida, cancellationToken);
            }
            else
            {
                rida = await _dbContext.TellimusedRida.FindAsync(request.Id);
                if (rida == null)
                {
                    result.AddError("Cannot find row with ID " + request.Id);
                    return result;
                }
            }

            rida.TellimusId = request.TellimusId;
            rida.ToodeId = request.ToodeId;
            rida.Quantity = request.Quantity;
            rida.UnitPrice = request.UnitPrice;
            rida.LineTotal = request.LineTotal;
            rida.VatRate = request.VatRate;
            rida.VatAmount = request.VatAmount;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}