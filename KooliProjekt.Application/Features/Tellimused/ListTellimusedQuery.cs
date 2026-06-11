using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Tellimused
{
[ExcludeFromCodeCoverage]
    public class ListTellimusedQuery : IRequest<OperationResult<PagedResult<TellimusListDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}