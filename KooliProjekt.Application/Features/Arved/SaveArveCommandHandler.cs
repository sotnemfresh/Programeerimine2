using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Arved
{
    public class SaveArveCommandHandler : IRequestHandler<SaveArveCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveArveCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveArveCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            var arve = request.Id == 0 ? new Arve() : await _dbContext.Arved.FindAsync(request.Id);

            if (request.Id == 0)
                await _dbContext.Arved.AddAsync(arve);

            arve.InvoiceNumber = request.InvoiceNumber;
            arve.InvoiceDate = request.InvoiceDate;
            arve.DueDate = request.DueDate;
            arve.Status = request.Status;
            arve.SubTotal = request.SubTotal;
            arve.ShippingTotal = request.ShippingTotal;
            arve.Discount = request.Discount;
            arve.GrandTotal = request.GrandTotal;
            arve.KlientId = request.KlientId;
            arve.TellimusId = request.TellimusId;

            await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}