using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Arved
{
    public class GetArveQueryHandler : IRequestHandler<GetArveQuery, OperationResult<ArveDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetArveQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<ArveDto>> Handle(GetArveQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<ArveDto>();

            var arve = await _dbContext.Arved
                .Include(a => a.Klient)
                .Include(a => a.Tellimus)
                    .ThenInclude(t => t.TellimuseRead)
                        .ThenInclude(r => r.Toode)
                .Where(a => a.Id == request.Id)
                .Select(a => new ArveDto
                {
                    Id = a.Id,
                    InvoiceNumber = a.InvoiceNumber,
                    InvoiceDate = a.InvoiceDate,
                    DueDate = a.DueDate,
                    Status = a.Status,
                    SubTotal = a.SubTotal,
                    ShippingTotal = a.ShippingTotal,
                    Discount = a.Discount,
                    GrandTotal = a.GrandTotal,
                    Klient = new KlientDto
                    {
                        Id = a.Klient.Id,
                        FirstName = a.Klient.FirstName,
                        LastName = a.Klient.LastName,
                        Address = a.Klient.Address,
                        Email = a.Klient.Email,
                        Phone = a.Klient.Phone,
                        Discount = a.Klient.Discount
                    },
                    Tellimus = new TellimusListDto
                    {
                        Id = a.Tellimus.Id,
                        OrderDate = a.Tellimus.OrderDate,
                        Status = a.Tellimus.Status,
                        KlientFirstName = a.Tellimus.Klient.FirstName,
                        KlientLastName = a.Tellimus.Klient.LastName,
                        RidadeArv = a.Tellimus.TellimuseRead.Count
                    }
                })
                .FirstOrDefaultAsync();

            result.Value = arve;
            return result;
        }
    }
}