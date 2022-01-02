using Microsoft.EntityFrameworkCore;

namespace EManager.Model
{
    public class EmanDbContext : DbContext
    {
        public EmanDbContext(DbContextOptions<EmanDbContext> options)
            :base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

    }
}
