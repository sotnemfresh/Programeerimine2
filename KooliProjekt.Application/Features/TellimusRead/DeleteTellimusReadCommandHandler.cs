using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.TellimuseRead
{
    public class DeleteTellimuseReadCommandHandler : IRequestHandler<DeleteTellimuseReadCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteTellimuseReadCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteTellimuseReadCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext.TellimusedRida
                .Where(r => r.Id == request.Id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}