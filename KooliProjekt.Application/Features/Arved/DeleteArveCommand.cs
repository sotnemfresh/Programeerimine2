using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Arved
{
    public class DeleteArveCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}