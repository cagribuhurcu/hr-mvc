using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HRProject.Entities.Validation.DateYearControlValidation;

namespace HRProject.Entities.Validation
{
    public static class QuitDateValidation
    {

        public static bool ValidateQuitDate(DateTime? quitDate)
        {
            if (quitDate is not null)
            {
                if (Control(quitDate) == true)
                {
                    return true;
                }
                else { return false; }
            }
            else
            {
                return true;
            }
        }
    }
}
