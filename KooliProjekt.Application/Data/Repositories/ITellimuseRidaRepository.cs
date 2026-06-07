using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    // See liides pärib ainult IBaseRepository-st ja ei vaja oma List-meetodit!
    public interface ITellimuseRidaRepository : IBaseRepository<TellimuseRida>
    {
    }
}