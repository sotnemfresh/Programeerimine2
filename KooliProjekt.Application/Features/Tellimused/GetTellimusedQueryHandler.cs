using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tellimused
{
    public class GetTellimusQueryHandler : IRequestHandler<GetTellimusQuery, OperationResult<TellimusDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetTellimusQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<TellimusDto>> Handle(GetTellimusQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<TellimusDto>();

            var tellimus = await _dbContext.Tellimused
                .Include(t => t.Klient)
                .Include(t => t.TellimuseRead)
                    .ThenInclude(r => r.Toode)
                .Include(t => t.Arve)
                .Where(t => t.Id == request.Id)
                .Select(t => new TellimusDto
                {
                    Id = t.Id,
                    OrderDate = t.OrderDate,
                    Status = t.Status,
                    Klient = new KlientListDto
                    {
                        Id = t.Klient.Id,
                        FirstName = t.Klient.FirstName,
                        LastName = t.Klient.LastName,
                        Email = t.Klient.Email,
                        Discount = t.Klient.Discount
                    },
                    Arve = t.Arve != null ? new ArveListDto
                    {
                        Id = t.Arve.Id,
                        InvoiceNumber = t.Arve.InvoiceNumber,
                        InvoiceDate = t.Arve.InvoiceDate,
                        GrandTotal = t.Arve.GrandTotal,
                        Status = t.Arve.Status,
                        Klient = new KlientListDto
                        {
                            Id = t.Arve.Klient.Id,
                            FirstName = t.Arve.Klient.FirstName,
                            LastName = t.Arve.Klient.LastName,
                            Email = t.Arve.Klient.Email,
                            Discount = t.Arve.Klient.Discount
                        }
                    } : null,
                    TellimuseRead = t.TellimuseRead.Select(r => new TellimuseRidaDto
                    {
                        Id = r.Id,
                        Quantity = r.Quantity,
                        UnitPrice = r.UnitPrice,
                        LineTotal = r.LineTotal,
                        TellimusId = t.Id,
                        Toode = new ToodeListDto
                        {
                            Id = r.Toode.Id,
                            Name = r.Toode.Name,
                            FotoURL = r.Toode.FotoURL,
                            Price = r.Toode.Price,
                            StockQuantity = r.Toode.StockQuantity
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            result.Value = tellimus;
            return result;
        }
    }
}