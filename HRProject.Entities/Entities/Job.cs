using HRProject.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Entities
{
    public class Job : BaseEntity
    {
        public Job()
        {
            siteManagers = new List<SiteManager>();
            CompanyManagers = new List<CompanyManagerEntity>();
        }

        public string Name { get; set; }
        public Departments Department { get; set; }
        public List<SiteManager> siteManagers { get; set; }
        public List<CompanyManagerEntity> CompanyManagers { get; set; }
    }
}
