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
        public string Name { get; set; }
        public Departments Department { get; set; }
    }
}
