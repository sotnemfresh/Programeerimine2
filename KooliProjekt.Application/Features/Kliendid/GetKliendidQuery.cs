using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Kliendid
{
[ExcludeFromCodeCoverage]
    public class GetKlientQuery : IRequest<OperationResult<KlientDto>>
    {
        public int Id { get; set; }
    }
}