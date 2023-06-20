using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HRProject.Entities.Enums
{
    public enum PermissionName
    {
        [Display(Name = "Paternity Leave")]
        PaternityLeave = 1,
        [Display(Name = "Maternity Leave")]
        MaternityLeave = 2,
        [Display(Name = "Breastfeeding Leave")]
        BreastfeedingLeave = 3,
        [Display(Name = "Death Leave")]
        DeathLeave = 4,
        [Display(Name = "Excuse Leave")]
        ExcuseLeave = 5,
        [Display(Name = "Marriage Leave")]
        MarriageLeave = 6,
        [Display(Name = "Annual Leave")]
        AnnualLeave = 7,

    }
}
