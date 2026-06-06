using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tooted
{
    public class GetToodeQuery : IRequest<OperationResult<ToodeDto>>
    {
        public int Id { get; set; }
    }
}