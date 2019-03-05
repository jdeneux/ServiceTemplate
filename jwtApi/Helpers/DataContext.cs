using jwtApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace jwtApi.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
