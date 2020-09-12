using AutoMapper.Configuration;
using CadUserAPI.ApplicationCore.Context;
using CadUserAPI.ApplicationCore.Models;
using CadUserAPI.ApplicationCore.Repositories.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CadUserAPI.ApplicationCore.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly string _connectionString;
        public UsuarioRepository(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
            _connectionString = "Server=DESKTOP-30MBPV6\\SQLEXPRESS;Database=CadUserDB;Trusted_Connection=True;";
        }

        public async Task AddAsync(Usuario entity)
        {
            await _appDbContext.Users.AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(dynamic id)
        {
            var user = await GetAsync(id);
            _appDbContext.Users.Remove(user);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var sql = "SELECT * FROM dbo.AspNetUsers";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<Usuario>(sql);
                return result;
            }
        }

        public async Task<Usuario> GetAsync(dynamic id)
        {
            var sql = "SELECT * FROM dbo.AspNetUsers WHERE UserId = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<Usuario>(sql, new { Id = id });
                return result.FirstOrDefault();
            }
        }

        public async Task UpdateAsync(Usuario entity)
        {
            entity.Modified = DateTime.Now;
            _appDbContext.Users.Update(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            var sql = "SELECT * FROM dbo.AspNetUsers WHERE Email = @Email";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<Usuario>(sql, new { Email = email });
                return result.FirstOrDefault();
            }
        }

        public async Task<Usuario> GetByToken(string token)
        {
            var sql = "SELECT * FROM dbo.AspNetUsers WHERE Last_Token = @Token";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<Usuario>(sql, new { Token = token });
                return result.FirstOrDefault();
            }
        }

        public async Task UpdateToken(Usuario usuario, string token)
        {
            usuario.Last_Token = token;
            await UpdateAsync(usuario);
        }
    }
}
