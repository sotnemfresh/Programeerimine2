﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Data.Repositories; 
using MediatR;
namespace KooliProjekt.Application.Features.Tooted
{
    public class ListTootedQueryHandler : IRequestHandler<ListTootedQuery, OperationResult<PagedResult<ToodeListDto>>>
    {
        private readonly IToodeRepository _toodeRepository;

        // Uuenda konstruktor
        public ListTootedQueryHandler(IToodeRepository toodeRepository)
        {
            _toodeRepository = toodeRepository;
        }

        public async Task<OperationResult<PagedResult<ToodeListDto>>> Handle(ListTootedQuery request, CancellationToken cancellationToken)
        {
            // Kutsuge välja repositori meetod, mis teeb kogu töö
            var pagedDtoResult = await _toodeRepository.GetPagedListAsync(request.Page, request.PageSize);

            // Tagastage edukas tulemus
            return OperationResult<PagedResult<ToodeListDto>>.Success(pagedDtoResult);
        }
    }
}