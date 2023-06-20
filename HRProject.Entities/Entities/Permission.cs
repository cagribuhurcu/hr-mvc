using HRProject.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Entities
{
    public class Permission:BaseEntity
    {
        public Permission()
        {
            EmployeePermissions=new List<EmployeePermission>();
        }
        public PermissionName PermissionName { get; set; }

        public int? MaxPermissionDay { get; set; }

        public List<EmployeePermission> EmployeePermissions { get; set; }

    }
}
