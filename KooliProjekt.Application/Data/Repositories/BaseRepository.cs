using System;
using System.Linq; 
using System.Threading.Tasks;
using KooliProjekt.Application.Infrastructure.Paging; 
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Application.Data; 

namespace KooliProjekt.Application.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        // Kasutame 'protected', et päritud klassid (nt KlientRepository) näeksid seda
        protected ApplicationDbContext DbContext { get; private set; }

        public BaseRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public async Task<PagedResult<T>> GetPageAsync(int page, int pageSize)
        {
            // Kasutab PagingExtensions.cs failis defineeritud laiendusmeetodit
            return await DbContext.Set<T>().GetPagedAsync(page, pageSize);
        }

        public async Task SaveAsync(T entity)
        {
            if (entity.Id != 0)
            {
                DbContext.Set<T>().Update(entity);
            }
            else
            {
                await DbContext.Set<T>().AddAsync(entity);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }
    }
}