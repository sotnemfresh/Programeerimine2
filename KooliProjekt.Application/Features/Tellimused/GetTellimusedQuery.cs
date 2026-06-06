using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tellimused
{
    public class GetTellimusQuery : IRequest<OperationResult<TellimusDto>>
    {
        public int Id { get; set; }
    }
}