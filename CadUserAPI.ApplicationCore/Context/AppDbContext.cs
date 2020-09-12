using CadUserAPI.ApplicationCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CadUserAPI.ApplicationCore.Context
{
    public class AppDbContext : IdentityDbContext<Usuario>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
