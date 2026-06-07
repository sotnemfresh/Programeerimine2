using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Dto;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IToodeRepository : IBaseRepository<Toode>
    {
        // Query meetodid
        Task<PagedResult<ToodeListDto>> GetPagedListAsync(int page, int pageSize);
        Task<ToodeDto> GetToodeDetailsDtoAsync(int id);

        // Command meetodid (Kasutame BaseRepository meetodeid otse)
    }
}