using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tooted
{
    public class SaveToodeCommandHandler : IRequestHandler<SaveToodeCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveToodeCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new System.ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveToodeCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new System.ArgumentNullException(nameof(request));
            var result = new OperationResult();
            if (request.Id < 0)
            {
                result.AddError("Request ID cannot be negative");
                return result;
            }

            Toode toode;

            if (request.Id == 0)
            {
                toode = new Toode();
                await _dbContext.Tooted.AddAsync(toode, cancellationToken);
            }
            else
            {
                toode = await _dbContext.Tooted.FindAsync(request.Id);
                if (toode == null)
                {
                    result.AddError("Cannot find product with ID " + request.Id);
                    return result;
                }
            }

            toode.Name = request.Name;
            toode.Description = request.Description;
            toode.FotoURL = request.FotoURL;
            toode.Price = request.Price;
            toode.StockQuantity = request.StockQuantity;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}