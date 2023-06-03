using HRProject.Entities.Entities;
using Microsoft.EntityFrameworkCore;

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
            optionsBuilder.UseSqlServer("Server=tcp:ikkaynak.database.windows.net,1433;Initial Catalog=ProjectDatabase;Persist Security Info=False;User ID=ikkaynak;Password=admin123.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        //DBSetler bu kısımın altında olacak
        public DbSet<Job> Jobs { get; set; }
        public DbSet<User> Users { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Job>().HasData(

        //        new Job
        //        {
                   
        //            Name = "Software Developer",
        //            Department = Entities.Enums.Departments.InformationSystems,
                    
        //        },
        //        new Job
        //        {
                    
        //            Name = "Front-End Developer",
        //            Department = Entities.Enums.Departments.InformationSystems,
        //        },
        //        new Job
        //        {
                   
        //            Name = "Back-End Developer",
        //            Department = Entities.Enums.Departments.InformationSystems,
        //        },
        //         new Job
        //         {
                    
        //             Name = "HR Manager",
        //             Department = Entities.Enums.Departments.HumanResources,
        //         },
        //         new Job
        //         {
                     
        //             Name = "HR Specialist",
        //             Department = Entities.Enums.Departments.HumanResources,
        //         },
        //          new Job
        //          {
                      
        //              Name = "Finance Manager",
        //              Department = Entities.Enums.Departments.Finance,
        //          },
        //          new Job
        //          {
                    
        //              Name = "Finance Expert",
        //              Department = Entities.Enums.Departments.Finance,
        //          },
        //          new Job
        //          {
                      
        //              Name = "Sales Engineer",
        //              Department = Entities.Enums.Departments.Sales,
        //          },
        //          new Job
        //          {
                      
        //              Name = "Sales Manager",
        //              Department = Entities.Enums.Departments.Sales,
        //          },
        //          new Job
        //          {
                      
        //              Name = "Marketing Manager",
        //               Department = Entities.Enums.Departments.Marketing,
        //          },
        //          new Job
        //          {
                      
        //              Name = "Social Media Specialist",
        //              Department = Entities.Enums.Departments.Marketing,
        //          }

        //        ) ;
        //    modelBuilder.Entity<User>().HasData(
        //        new User
        //        {
        //            ID = 10,
        //            FirstName = "John",
        //            MiddleName = "Jane",
        //            LastName = "Doe",
        //            BirthPlace = "Yozgat",
        //            BirthDate = new DateTime(1996, 05, 07),
        //            HireDate = new DateTime(2020, 01, 01),
        //            IdentificationNumber = "92874132130",
        //            Address = "Çandır/Yozgat",
        //            PhoneNumber = "5375681245",
        //            Department = Entities.Enums.Departments.InformationSystems,
        //            Role = Entities.Enums.Roles.SiteManager,
        //            JobID = 3,

        //        });

        //}
    }
}
