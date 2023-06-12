using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Enums
{
    public enum Titles
    {
        [Display(Name = "Limited Company")]
        LimitedCompany = 1,

        [Display(Name = "Incorporated")]
        Incorporated,

        [Display(Name = "Corporation")]
        Corporation,

        [Display(Name = "Company")]
        Company,

        [Display(Name = "Partnership")]
        Partnership,

        [Display(Name = "Sole Proprietorship")]
        SoleProprietorship,

        [Display(Name = "Public Limited Company")]
        PublicLimitedCompany,

        [Display(Name = "Nonprofit Organization")]
        NonprofitOrganization,

        [Display(Name = "Cooperative")]
        Cooperative,

        [Display(Name = "Limited Liability Company")]
        LimitedLiabilityCompany

    }
}
