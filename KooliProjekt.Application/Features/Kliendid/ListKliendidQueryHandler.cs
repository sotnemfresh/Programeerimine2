﻿﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;

namespace KooliProjekt.Application.Features.Kliendid
{
    public class ListKliendidQueryHandler : IRequestHandler<ListKliendidQuery, OperationResult<PagedResult<KlientListDto>>>
    {
        private readonly IKlientRepository _klientRepository;

        public ListKliendidQueryHandler(IKlientRepository klientRepository)
        {
            _klientRepository = klientRepository;
        }

        public async Task<OperationResult<PagedResult<KlientListDto>>> Handle(ListKliendidQuery request, CancellationToken cancellationToken)
        {
            // 1. Kasutame GetPageAsync meetodit. See tagastab PagedResult<Klient> (Entityd)
            var pagedEntities = await _klientRepository.GetPageAsync(request.Page, request.PageSize);

            // 2. Map Klient Entity list KlientListDto listiks
            var dtoList = new List<KlientListDto>();

            foreach (var entity in pagedEntities.Results)
            {
                dtoList.Add(new KlientListDto
                {
                    Id = entity.Id,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Email = entity.Email,
                });
            }

            // 3. Loo uus PagedResult DTO-dega, aga säilita Paging info
            var pagedDtoResult = new PagedResult<KlientListDto>
            {
                Results = dtoList, 
                CurrentPage = pagedEntities.CurrentPage, 
                PageSize = pagedEntities.PageSize,
                RowCount = pagedEntities.RowCount, 
                PageCount = pagedEntities.PageCount // Vajalik, et PagedResult oleks terviklik
            };

            // 4. Tagasta edukas OperationResult koos õige PagedResult<KlientListDto> tüübiga
            return OperationResult<PagedResult<KlientListDto>>.Success(pagedDtoResult);
        }
    }
}