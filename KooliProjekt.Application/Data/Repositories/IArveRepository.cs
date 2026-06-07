using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Dto;
using System.Threading.Tasks;
using KooliProjekt.Application.Data; 
using KooliProjekt.Application.Features.Arved; 

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IArveRepository : IBaseRepository<Arve>
    {
        // Query meetodid
        // 1. ListHandler: Vajalik Kliendi andmetega leheküljendatud DTO loendi jaoks
        Task<PagedResult<ArveListDto>> GetPagedListAsync(int page, int pageSize);

        // 2. GetHandler: Vajalik väga detailse ArveDto (koos Tellimuse ja ridadega) jaoks
        Task<ArveDto> GetArveDetailsDtoAsync(int id);

        // Command meetodid (Save/Delete saavad kasutada BaseRepository meetodeid)
    }
}