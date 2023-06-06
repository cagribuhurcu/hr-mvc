using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Enums
{
    public enum Titles
    {
        [Description("Limited Company")]
        LimitedCompany =1, //(Ltd.)
        Incorporated, //(Inc.)
        Corporation, //(Corp.)
        Company, //(Co.)
        Partnership,
        [Description("Sole Proprietorship")]
        SoleProprietorship,
        [Description("Public Limited Company")]
        PublicLimitedCompany, //(PLC)
        [Description("Nonprofit Organization")]
        NonprofitOrganization,
        Cooperative, //(Co-op)
        [Description("Limited Liability Company")]
        LimitedLiabilityCompany //(LLC)

    }
}
