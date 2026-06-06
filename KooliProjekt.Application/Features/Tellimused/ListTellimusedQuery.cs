using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tellimused
{
    public class ListTellimusedQuery : IRequest<OperationResult<PagedResult<TellimusListDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}