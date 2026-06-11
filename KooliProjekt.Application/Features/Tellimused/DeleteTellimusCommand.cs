using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Tellimused
{
[ExcludeFromCodeCoverage]
    public class DeleteTellimusCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}