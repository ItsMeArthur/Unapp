using Microsoft.EntityFrameworkCore;
using Una.Presentation.API.Entities;

namespace Una.Presentation.API.Context
{
    public class UnaDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }

        public UnaDbContext(DbContextOptions<UnaDbContext> options) : base(options)
        {
        }
    }
}
