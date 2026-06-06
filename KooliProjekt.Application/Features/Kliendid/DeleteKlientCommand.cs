using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Kliendid
{
    public class DeleteKlientCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}