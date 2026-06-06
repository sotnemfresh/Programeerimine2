﻿using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using KooliProjekt.Application.Dto;
namespace KooliProjekt.Application.Features.Kliendid
{
    public class ListKliendidQuery : IRequest<OperationResult<PagedResult<KlientListDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}