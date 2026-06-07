using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Data.Repositories
{
    public class ToodeRepository : BaseRepository<Toode>, IToodeRepository
    {
        public ToodeRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        // Olemasolev List meetod
        public async Task<PagedResult<ToodeListDto>> GetPagedListAsync(int page, int pageSize)
        {
            var query = DbContext.Set<Toode>()
                .OrderBy(t => t.Name)
                .Select(t => new ToodeListDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    FotoURL = t.FotoURL,
                    Price = t.Price,
                    StockQuantity = t.StockQuantity
                });

            return await query.GetPagedAsync(page, pageSize);
        }

        //  GetToodeDetailsDtoAsync implementatsioon
        public async Task<ToodeDto> GetToodeDetailsDtoAsync(int id)
        {
            var toodeDto = await DbContext.Set<Toode>()
                .Where(t => t.Id == id)
                .Select(t => new ToodeDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    FotoURL = t.FotoURL,
                    Price = t.Price,
                    StockQuantity = t.StockQuantity
                })
                .FirstOrDefaultAsync();

            return toodeDto;
        }
    }
}