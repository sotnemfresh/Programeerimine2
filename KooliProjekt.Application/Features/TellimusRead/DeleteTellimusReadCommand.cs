using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.TellimuseRead
{
    public class DeleteTellimuseReadCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}