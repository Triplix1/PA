using Microsoft.EntityFrameworkCore;

namespace Lab3.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Row> Rows { get; set; }
    }
}
