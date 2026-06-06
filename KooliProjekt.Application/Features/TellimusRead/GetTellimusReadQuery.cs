﻿using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.TellimuseRead
{
    public class GetTellimuseReadQuery : IRequest<OperationResult<TellimuseRidaDto>>
    {
        public int Id { get; set; }
    }
}