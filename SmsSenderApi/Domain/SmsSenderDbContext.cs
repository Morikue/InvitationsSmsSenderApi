using SmsSenderApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SmsSenderApi.Domain
{
    /// <summary>
    /// Контекст для Sql Server
    /// </summary>
    public class SmsSenderDbContext : DbContext
    {
        public SmsSenderDbContext(DbContextOptions<SmsSenderDbContext> opt) 
            : base(opt){}

        public DbSet<Message> Messages { get; set; }

    }
}
