using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Arved;
using System.Collections.Generic;

namespace KooliProjekt.Application.Data.Repositories
{
    public class ArveRepository : BaseRepository<Arve>, IArveRepository
    {
        public ArveRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        // 1. Implementatsioon ListArvedQueryHandler jaoks (Include Klient)
        public async Task<PagedResult<ArveListDto>> GetPagedListAsync(int page, int pageSize)
        {
            var query = DbContext.Set<Arve>()
                .Include(a => a.Klient)
                .OrderBy(a => a.InvoiceDate)
                .Select(a => new ArveListDto
                {
                    Id = a.Id,
                    InvoiceNumber = a.InvoiceNumber,
                    InvoiceDate = a.InvoiceDate,
                    GrandTotal = a.GrandTotal,
                    Status = a.Status,
                    Klient = new KlientListDto
                    {
                        Id = a.Klient.Id,
                        FirstName = a.Klient.FirstName,
                        LastName = a.Klient.LastName,
                        Email = a.Klient.Email,
                        Discount = a.Klient.Discount
                    }
                });

            return await query.GetPagedAsync(page, pageSize);
        }

        // 2. Implementatsioon GetArveQueryHandler jaoks (Väga keeruline Include ahel)
        public async Task<ArveDto> GetArveDetailsDtoAsync(int id)
        {
            var arveDto = await DbContext.Arved
                .Include(a => a.Klient)
                .Include(a => a.Tellimus)
                    .ThenInclude(t => t.TellimuseRead)
                        .ThenInclude(r => r.Toode)
                .Where(a => a.Id == id)
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
                        KlientFirstName = a.Tellimus.Klient.FirstName, // See on tegelikult dubleeritud, aga jätame DTO mappingu samaks
                        KlientLastName = a.Tellimus.Klient.LastName, // Sest TellimusEntity laeti Include kaudu sisse
                        RidadeArv = a.Tellimus.TellimuseRead.Count
                    }
                })
                .FirstOrDefaultAsync();

            return arveDto;
        }
    }
}