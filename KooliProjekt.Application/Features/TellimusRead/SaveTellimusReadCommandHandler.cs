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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveTellimuseReadCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var rida = request.Id == 0 ? new TellimuseRida() : await _dbContext.TellimusedRida.FindAsync(request.Id);
            if (request.Id == 0) await _dbContext.TellimusedRida.AddAsync(rida);

            rida.TellimusId = request.TellimusId;
            rida.ToodeId = request.ToodeId;
            rida.Quantity = request.Quantity;
            rida.UnitPrice = request.UnitPrice;
            rida.LineTotal = request.LineTotal;
            rida.VatRate = request.VatRate;
            rida.VatAmount = request.VatAmount;

            await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}