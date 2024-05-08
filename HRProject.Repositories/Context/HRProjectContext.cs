using HRProject.Entities.Entities;
using Microsoft.EntityFrameworkCore;


namespace HRProject.Repositories.Context
{
    public class HRProjectContext : DbContext
    {
        public HRProjectContext()
        {
            
        }
        public HRProjectContext(DbContextOptions options) : base(options)
        {

        }


        //Azure Database Bağlantısı İçin
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=DESKTOP-OEIFO1O\\CAGRISERVER; Database=GalaxyHrProject; Uid=sa; Pwd=1234;");
        }

        //DBSetler bu kısımın altında olacak
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<SiteManager> SiteManagers { get; set; }
        public DbSet<CompanyManagerEntity> CompanyManagers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<EmployeePermission> EmployeePermissions { get; set; }
        public DbSet<Expense> Expenses { get; set; }

    }
}
