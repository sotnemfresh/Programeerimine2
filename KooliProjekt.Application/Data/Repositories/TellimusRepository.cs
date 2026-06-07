using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using Microsoft.EntityFrameworkCore;
using System.Linq; // Vajalik LINQ/FirstOrDefault/Where jaoks
using System.Threading.Tasks; // Vajalik Task jaoks
using KooliProjekt.Application.Data;
using System.Collections.Generic; // Vajalik List<T> jaoks
using KooliProjekt.Application.Features.Tellimused; // Vajalik SaveTellimusCommand jaoks

namespace KooliProjekt.Application.Data.Repositories
{
    public class TellimusRepository : BaseRepository<Tellimus>, ITellimusRepository
    {
        public TellimusRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<PagedResult<TellimusListDto>> GetPagedListAsync(int page, int pageSize)
        {
            var query = DbContext.Set<Tellimus>()
                .Include(t => t.Klient)
                .Include(t => t.TellimuseRead)
                .OrderByDescending(t => t.OrderDate)
                .Select(t => new TellimusListDto
                {
                    Id = t.Id,
                    OrderDate = t.OrderDate,
                    Status = t.Status,
                    KlientFirstName = t.Klient.FirstName,
                    KlientLastName = t.Klient.LastName,
                    RidadeArv = t.TellimuseRead.Count
                });

            return await query.GetPagedAsync(page, pageSize);
        }

        public async Task<TellimusDto> GetTellimusDetailsDtoAsync(int id)
        {
            // Pikk LINQ päring... (ei muuda)
            var tellimus = await DbContext.Tellimused
               .Include(t => t.Klient)
               .Include(t => t.TellimuseRead)
                   .ThenInclude(r => r.Toode)
               .Include(t => t.Arve)
               // Kogu select loogika, mis tagastab TellimusDto
               .Where(t => t.Id == id)
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

            return tellimus;
        }

        //  Laeb TellimuseRead andmed DbContext-i
        public async Task LoadTellimuseReadAsync(Tellimus tellimus)
        {
            await DbContext.Entry(tellimus).Collection(t => t.TellimuseRead).LoadAsync();
        }

        public async Task SaveTellimusDetailsAsync(Tellimus tellimus, SaveTellimusCommand command)
        {
            // 1. Uuenda Tellimuse põhiväljad
            tellimus.OrderDate = command.OrderDate;
            tellimus.Status = command.Status;
            tellimus.KlientId = command.KlientId;

            // Kontrolli, kas tegemist on uue tellimusega
            if (tellimus.Id == 0)
            {
                // Lisame tellimuse ja selle ridade kogumi
                tellimus.TellimuseRead = new List<TellimuseRida>(); // Tagab, et meil on olemas List
                DbContext.Set<Tellimus>().Add(tellimus);
            }

            // 2. Uuenda või lisa TellimuseRead
            // Eemaldame read, mida Command ei sisalda
            var ridadeIdsInCommand = command.TellimuseRead.Where(r => r.Id != 0).Select(r => r.Id).ToList();
            var ridadToBeDeleted = tellimus.TellimuseRead.Where(r => !ridadeIdsInCommand.Contains(r.Id)).ToList();

            // Kustutame read, mida enam ei soovita
            foreach (var rida in ridadToBeDeleted)
            {
                tellimus.TellimuseRead.Remove(rida);
                DbContext.Set<TellimuseRida>().Remove(rida);
            }

            // Uuendame olemasolevaid või lisame uued read
            foreach (var ridaDto in command.TellimuseRead)
            {
                var rida = tellimus.TellimuseRead.FirstOrDefault(tr => tr.Id == ridaDto.Id) ?? new TellimuseRida();

                if (rida.Id == 0)
                {
                    // Uus rida, lisame Tellimuse Read listi
                    tellimus.TellimuseRead.Add(rida);
                }

                // Mapime TellimuseRida andmed DTO-lt Entity-sse
                rida.ToodeId = ridaDto.ToodeId;
                rida.Quantity = ridaDto.Quantity;
                rida.UnitPrice = ridaDto.UnitPrice;
                rida.LineTotal = ridaDto.LineTotal;
                rida.VatRate = ridaDto.VatRate;
                rida.VatAmount = ridaDto.VatAmount;
            }

            // 3. Salvesta kõik muudatused (Tellimus, lisatud/uuendatud/kustutatud read)
            await DbContext.SaveChangesAsync();
        }
    }
}