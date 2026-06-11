using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.TellimuseRead
{
[ExcludeFromCodeCoverage]
    public class DeleteTellimuseReadCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}