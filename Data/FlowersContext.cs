
using flowers.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace flowers.api.Data
{
    public class FlowersContext : DbContext
    {
        public DbSet<Flower> Flowers { get; set; }
        public DbSet<Family> Families { get; set; }
        public FlowersContext(DbContextOptions options) : base(options) { }
    }
}