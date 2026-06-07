using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Data.Repositories; 
using MediatR;


namespace KooliProjekt.Application.Features.Tellimused
{
    public class ListTellimusedQueryHandler : IRequestHandler<ListTellimusedQuery, OperationResult<PagedResult<TellimusListDto>>>
    {
        // Asenda ApplicationDbContext ITellimusRepository-ga
        private readonly ITellimusRepository _tellimusRepository;

        // Uuenda konstruktor
        public ListTellimusedQueryHandler(ITellimusRepository tellimusRepository)
        {
            _tellimusRepository = tellimusRepository;
        }

        public async Task<OperationResult<PagedResult<TellimusListDto>>> Handle(ListTellimusedQuery request, CancellationToken cancellationToken)
        {
            // Kutsuge välja spetsiaalne repositori meetod, mis teeb kogu LINQ töö
            var pagedDtoResult = await _tellimusRepository.GetPagedListAsync(request.Page, request.PageSize);

            // Tagastage edukas tulemus
            return OperationResult<PagedResult<TellimusListDto>>.Success(pagedDtoResult);
        }
    }
}