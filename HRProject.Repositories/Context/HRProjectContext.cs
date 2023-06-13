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
            optionsBuilder.UseSqlServer("Server=tcp:ikkaynak2.database.windows.net,1433;Initial Catalog=Database;Persist Security Info=False;User ID=ikkaynak;Password=admin123.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        //DBSetler bu kısımın altında olacak
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<SiteManager> SiteManagers { get; set; }
        public DbSet<CompanyManagerEntity> CompanyManagers { get; set; }
        public DbSet<Employee> Employees { get; set; }

    }
}
