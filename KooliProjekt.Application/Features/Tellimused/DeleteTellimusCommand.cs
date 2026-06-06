using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tellimused
{
    public class DeleteTellimusCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}