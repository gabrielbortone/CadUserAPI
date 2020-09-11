using CadUserAPI.ApplicationCore.Models;
using System.Threading.Tasks;

namespace CadUserAPI.ApplicationCore.Repositories.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        public Task<Usuario> GetByEmailAsync(string email);
    }
}
