using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Kliendid
{
    public class SaveKlientCommandHandler : IRequestHandler<SaveKlientCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveKlientCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveKlientCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var klient = request.Id == 0 ? new Klient() : await _dbContext.Kliendid.FindAsync(request.Id);
            if (request.Id == 0) await _dbContext.Kliendid.AddAsync(klient);

            klient.FirstName = request.FirstName;
            klient.LastName = request.LastName;
            klient.Address = request.Address;
            klient.Email = request.Email;
            klient.Phone = request.Phone;
            klient.Discount = request.Discount;

            await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}