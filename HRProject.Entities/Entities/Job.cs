﻿using HRProject.Entities.Enums;
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
            Users = new List<User>();
            CompanyManagers = new List<CompanyManagerEntity>();
        }

        public string Name { get; set; }
        public Departments Department { get; set; }
        public List<User> Users { get; set; }
        public List<CompanyManagerEntity> CompanyManagers { get; set; }
    }
}
