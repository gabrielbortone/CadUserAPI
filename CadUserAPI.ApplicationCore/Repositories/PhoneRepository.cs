using CadUserAPI.ApplicationCore.Context;
using CadUserAPI.ApplicationCore.Models;
using CadUserAPI.ApplicationCore.Repositories.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CadUserAPI.ApplicationCore.Repositories
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly string ConnectioString;
        public PhoneRepository(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
            var conn = _appDbContext.Database.GetDbConnection();
            ConnectioString = conn?.ConnectionString;
            conn?.Dispose();
        }
        public async Task AddAsync(Phone entity)
        {
            await _appDbContext.AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(dynamic id)
        {
            var phone = await GetAsync(id);
            await _appDbContext.Remove(phone);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Phone>> GetAllAsync()
        {
            var sql = "SELECT * FROM dbo.Phones";
            using (var connection = new SqlConnection(ConnectioString))
            {
                connection.Open();
                var result = await connection.QueryAsync<Phone>(sql);
                return result;
            }
        }

        public async Task<Phone> GetAsync(dynamic id)
        {
            var sql = "SELECT * FROM dbo.Phone WHERE dbo.PhoneId = @Id";
            using (var connection = new SqlConnection(ConnectioString))
            {
                connection.Open();
                var result = await connection.QueryAsync<Phone>(sql, new { Id = id });
                return result.FirstOrDefault();
            }
        }

        public async Task UpdateAsync(Phone entity)
        {
            _appDbContext.Phones.Update(entity);
            await _appDbContext.SaveChangesAsync();
        }
    }
}