using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class TellimuseRidaRepository : BaseRepository<TellimuseRida>, ITellimuseRidaRepository
    {
        public TellimuseRidaRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        // Eraldi meetodeid List-Query jaoks pole vaja
    }
}