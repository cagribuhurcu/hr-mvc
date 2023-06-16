using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Validation
{
    public static class DateYearControlValidation
    {
        public static bool Control(DateTime? date)
        {
            if(date is not null)
            {
                int year = date.Value.Year;
                if (year >= 1800)
                {
                    return true;
                }
            }
            return false;   
        }
    }
}
