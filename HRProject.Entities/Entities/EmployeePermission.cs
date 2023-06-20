using HRProject.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Entities
{
    public class EmployeePermission:BaseEntity
    {

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public int PermissionId { get; set; }
        public Permission? Permission { get; set; }

        public PermissionState PermissionState { get; set; }

        public DateTime? RequiredDate { get; set; }

        public DateTime? ConfirmedDate { get; set; } //Company manager cevapladığında atanacak date

        public DateTime? StartDate { get; set; } 

        public DateTime? EndDate { get; set; } 

        public int? TotalRequiredDay { get; set; } 

    }
}
