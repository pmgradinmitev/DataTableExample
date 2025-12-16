using DataTableExample.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataTableExample.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Medicine> Medicines { get; set; }
    }
}
