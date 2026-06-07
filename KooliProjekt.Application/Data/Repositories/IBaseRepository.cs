using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    // Baasliides, millest kõik spetsiifilised I...Repository liidesed pärivad
    public interface IBaseRepository<T> where T : Entity
    {
        // Tagastab ühe kirje ID alusel
        Task<T> GetByIdAsync(int id);

        // Salvestab (lisab või uuendab) kirje
        Task SaveAsync(T entity);

        // Kustutab kirje
        Task DeleteAsync(T entity);
    }
}