using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Dto;
using System.Threading.Tasks;
using KooliProjekt.Application.Features.Tellimused; 
using KooliProjekt.Application.Data; 

namespace KooliProjekt.Application.Data.Repositories
{
    public interface ITellimusRepository : IBaseRepository<Tellimus>
    {
        // Query meetodid
        Task<PagedResult<TellimusListDto>> GetPagedListAsync(int page, int pageSize);
        Task<TellimusDto> GetTellimusDetailsDtoAsync(int id);

        // Command meetodid
        // Vajalik ridade laadimiseks enne uuendamist
        Task LoadTellimuseReadAsync(Tellimus tellimus);

        //  Tellimuse ja ridade salvestamiseks
        Task SaveTellimusDetailsAsync(Tellimus tellimus, SaveTellimusCommand command);
    }
}