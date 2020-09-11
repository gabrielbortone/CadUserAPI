using System.Collections.Generic;
using System.Threading.Tasks;

namespace CadUserAPI.ApplicationCore.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetAsync(dynamic id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task DeleteAsync(dynamic id);
        Task UpdateAsync(T entity);
    }
}
