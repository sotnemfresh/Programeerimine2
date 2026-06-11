using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Arved
{
[ExcludeFromCodeCoverage]
    public class DeleteArveCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}