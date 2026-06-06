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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveToodeCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var toode = request.Id == 0 ? new Toode() : await _dbContext.Tooted.FindAsync(request.Id);
            if (request.Id == 0) await _dbContext.Tooted.AddAsync(toode);

            toode.Name = request.Name;
            toode.Description = request.Description;
            toode.FotoURL = request.FotoURL;
            toode.Price = request.Price;
            toode.StockQuantity = request.StockQuantity;

            await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}