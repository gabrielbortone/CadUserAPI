using CadUserAPI.ApplicationCore.Context;
using CadUserAPI.ApplicationCore.Models;
using CadUserAPI.ApplicationCore.Repositories.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadUserAPI.ApplicationCore.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly string ConnectioString;
        public UsuarioRepository(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
            var conn = _appDbContext.Database.GetDbConnection();
            ConnectioString = conn?.ConnectionString;
            conn?.Dispose();
        }

        public async Task AddAsync(Usuario entity)
        {
            await _appDbContext.Usuarios.AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(dynamic id)
        {
            var user = await GetAsync(id);
            _appDbContext.Usuarios.Remove(user);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var sql = "SELECT * FROM dbo.Usuario";
            using (var connection = new SqlConnection(ConnectioString))
            {
                connection.Open();
                var result = await connection.QueryAsync<Usuario>(sql);
                return result;
            }
        }

        public async Task<Usuario> GetAsync(dynamic id)
        {
            var sql = "SELECT * FROM dbo.Usuario WHERE dbo.UserId = @Id";
            using (var connection = new SqlConnection(ConnectioString))
            {
                connection.Open();
                var result = await connection.QueryAsync<Usuario>(sql, new { Id = id });
                return result.FirstOrDefault();
            }
        }

        public async Task UpdateAsync(Usuario entity)
        {
            _appDbContext.Usuarios.Update(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            var sql = "SELECT * FROM dbo.Usuario WHERE dbo.Email = @Email";
            using (var connection = new SqlConnection(ConnectioString))
            {
                connection.Open();
                var result = await connection.QueryAsync<Usuario>(sql, new { Email = email });
                return result.FirstOrDefault();
            }
        }

       
    }
}
