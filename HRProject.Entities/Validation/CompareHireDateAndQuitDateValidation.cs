using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Validation
{
    public static class CompareHireDateAndQuitDateValidation
    {
        public static bool Compare(DateTime? hireDate, DateTime? quitDate)
        {
            if (hireDate is not null && quitDate is not null)
            {
                if (hireDate.Value < quitDate.Value)
                {
                    return true;
                }
            }
            else if(hireDate is not null && quitDate is null)
            {
                return true;
            }
            return false;
        }
    }
}
