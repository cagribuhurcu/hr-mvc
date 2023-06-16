using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HRProject.Entities.Validation.DateYearControlValidation;

namespace HRProject.Entities.Validation
{
    public static class HiringAgeValidation
    {
        public static bool ValidateAge(DateTime? birthDate, DateTime? hireDate)
        {
            if(birthDate is not null && hireDate is not null)
            { 
                if(Control(birthDate)==true && Control(hireDate) == true)
                {
                    DateTime minimumHireDate = hireDate.Value.AddYears(-18);
                    if (minimumHireDate >= birthDate)
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
    }
}
