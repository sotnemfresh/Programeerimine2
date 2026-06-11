using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;


namespace KooliProjekt.Application.Features.TellimuseRead
{
[ExcludeFromCodeCoverage]
    public class ListTellimuseReadQuery : IRequest<OperationResult<PagedResult<TellimuseRidaListDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}