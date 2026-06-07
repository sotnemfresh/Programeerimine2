using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tooted
{
    public class DeleteToodeCommandHandler : IRequestHandler<DeleteToodeCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteToodeCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult> Handle(DeleteToodeCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var result = new OperationResult();
            if (request.Id <= 0) return result;


            var toode = await _dbContext.Tooted
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (toode == null) return result;

            _dbContext.Tooted.Remove(toode);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}