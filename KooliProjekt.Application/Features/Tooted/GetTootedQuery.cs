using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Tooted
{
[ExcludeFromCodeCoverage]
    public class GetToodeQuery : IRequest<OperationResult<ToodeDto>>
    {
        public int Id { get; set; }
    }
}