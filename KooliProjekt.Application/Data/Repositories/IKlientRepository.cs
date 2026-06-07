using KooliProjekt.Application.Infrastructure.Paging;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    // Pärib üldistest CRUD meetoditest
    public interface IKlientRepository : IBaseRepository<Klient>
    {
        // Spetsiifiline meetod (Vajalik Klientide nimekirja saamiseks)
        Task<PagedResult<Klient>> GetPageAsync(int page, int pageSize);
    }
}