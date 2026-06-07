using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Arved
{
    public class DeleteArveCommandHandler : IRequestHandler<DeleteArveCommand, OperationResult>
    {
        private readonly IArveRepository _arveRepository;

        public DeleteArveCommandHandler(IArveRepository arveRepository)
        {
            _arveRepository = arveRepository;
        }

        public async Task<OperationResult> Handle(DeleteArveCommand request, CancellationToken cancellationToken)
        {
            var arveEntity = await _arveRepository.GetByIdAsync(request.Id);

            if (arveEntity == null)
            {
                return OperationResult.Failure($"Arvet ID {request.Id} ei leitud, kustutamine ebaõnnestus.");
            }

            await _arveRepository.DeleteAsync(arveEntity);

            return OperationResult.Success();
        }
    }
}