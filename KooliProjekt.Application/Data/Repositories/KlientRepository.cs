using KooliProjekt.Application.Infrastructure.Paging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public class KlientRepository : BaseRepository<Klient>, IKlientRepository
    {
        public KlientRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            // Konstruktor saadab DbContexti BaseRepository-le
        }

        // Rakendab IKlientRepository meetodit List-Query jaoks
        public async Task<PagedResult<Klient>> GetPageAsync(int page, int pageSize)
        {
            // Kasutame DbContexti siin otse, kuna see on spetsiifiline päring
            var query = DbContext.Set<Klient>().AsNoTracking();

            return await query.GetPagedAsync(page, pageSize);
        }

    }
}