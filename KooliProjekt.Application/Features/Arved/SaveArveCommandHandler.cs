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
            if (dbContext == null) throw new System.ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveArveCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new System.ArgumentNullException(nameof(request));

            var result = new OperationResult();
            if (request.Id < 0)
            {
                result.AddError("Request ID cannot be negative");
                return result;
            }

            Arve arve;

            if (request.Id == 0)
            {
                arve = new Arve();
                await _dbContext.Arved.AddAsync(arve, cancellationToken);
            }
            else
            {
                arve = await _dbContext.Arved.FindAsync(request.Id);
                if (arve == null)
                {
                    result.AddError("Cannot find invoice with ID " + request.Id);
                    return result;
                }
            }

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

            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}