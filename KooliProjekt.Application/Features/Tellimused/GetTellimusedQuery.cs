using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Tellimused
{
[ExcludeFromCodeCoverage]
    public class GetTellimusQuery : IRequest<OperationResult<TellimusDto>>
    {
        public int Id { get; set; }
    }
}