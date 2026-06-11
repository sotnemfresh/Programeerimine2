using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Kliendid
{
[ExcludeFromCodeCoverage]
    public class DeleteKlientCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}