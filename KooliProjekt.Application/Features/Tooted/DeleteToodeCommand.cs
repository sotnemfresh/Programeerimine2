using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tooted
{
    public class DeleteToodeCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}