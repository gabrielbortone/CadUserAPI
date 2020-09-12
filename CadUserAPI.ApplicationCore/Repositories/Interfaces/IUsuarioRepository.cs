using CadUserAPI.ApplicationCore.Models;
using System.Threading.Tasks;

namespace CadUserAPI.ApplicationCore.Repositories.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        public Task<Usuario> GetByEmailAsync(string email);
        public Task<Usuario> GetByToken(string token);
        public Task UpdateToken(Usuario usuario, string token);
    }
}
