using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Arved
{
    public class GetArveQuery : IRequest<OperationResult<ArveDto>>
    {
        public int Id { get; set; }
    }
}