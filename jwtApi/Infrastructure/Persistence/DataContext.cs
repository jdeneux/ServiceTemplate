using jwtApi.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace jwtApi.Infrastructure.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
