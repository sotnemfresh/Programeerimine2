using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Tooted
{
[ExcludeFromCodeCoverage]
    public class DeleteToodeCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}