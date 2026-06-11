﻿using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.TellimuseRead
{
[ExcludeFromCodeCoverage]
    public class GetTellimuseReadQuery : IRequest<OperationResult<TellimuseRidaDto>>
    {
        public int Id { get; set; }
    }
}