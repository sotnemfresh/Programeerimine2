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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveTellimusCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            var tellimus = request.Id == 0 ? new Tellimus() : await _dbContext.Tellimused
                .Include(t => t.TellimuseRead)
                .FirstOrDefaultAsync(t => t.Id == request.Id);

            if (request.Id == 0)
                await _dbContext.Tellimused.AddAsync(tellimus);

            tellimus.OrderDate = request.OrderDate;
            tellimus.Status = request.Status;
            tellimus.KlientId = request.KlientId;

            // Update or add TellimuseRead
            foreach (var ridaDto in request.TellimuseRead)
            {
                var rida = tellimus.TellimuseRead.FirstOrDefault(tr => tr.Id == ridaDto.Id) ?? new TellimuseRida();
                if (rida.Id == 0) tellimus.TellimuseRead.Add(rida);

                rida.ToodeId = ridaDto.ToodeId;
                rida.Quantity = ridaDto.Quantity;
                rida.UnitPrice = ridaDto.UnitPrice;
                rida.LineTotal = ridaDto.LineTotal;
                rida.VatRate = ridaDto.VatRate;
                rida.VatAmount = ridaDto.VatAmount;
            }

            await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}