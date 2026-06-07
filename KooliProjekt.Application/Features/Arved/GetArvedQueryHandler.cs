using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Arved
{
    public class GetArveQueryHandler : IRequestHandler<GetArveQuery, OperationResult<ArveDto>>
    {
        private readonly IArveRepository _arveRepository;

        public GetArveQueryHandler(IArveRepository arveRepository)
        {
            _arveRepository = arveRepository;
        }

        public async Task<OperationResult<ArveDto>> Handle(GetArveQuery request, CancellationToken cancellationToken)
        {
            var arveDto = await _arveRepository.GetArveDetailsDtoAsync(request.Id);

            if (arveDto == null)
            {
                return OperationResult<ArveDto>.Failure($"Arvet ID {request.Id} ei leitud.");
            }

            return OperationResult<ArveDto>.Success(arveDto);
        }
    }
}