using HRProject.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Repositories.Context
{
    public class HRProjectContext : DbContext 
    {
        public HRProjectContext(DbContextOptions options) : base(options)
        {
            
        }


        //Azure Database Bağlantısı İçin
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=tcp:ikkaynak.database.windows.net,1433;Initial Catalog=HRProjectDatabase;Persist Security Info=False;User ID=ikkaynak;Password=admin123.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        //DBSetler bu kısımın altında olacak
        public DbSet<User> Users { get; set; }
    }
}
