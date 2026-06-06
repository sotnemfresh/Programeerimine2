using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Kliendid
{
    public class GetKlientQuery : IRequest<OperationResult<KlientDto>>
    {
        public int Id { get; set; }
    }
}