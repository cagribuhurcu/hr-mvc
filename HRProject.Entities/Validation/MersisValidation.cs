using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HRProject.Entities.Validation.TaxNumberValidation;

namespace HRProject.Entities.Validation
{
    public static class MersisValidation
    {
        public static bool IsMersisNoValid(string mersisNo,string vergiNo)
        {
            if (mersisNo.Length != 16)
            {
                return false;
            }
            else if(mersisNo.Length==16)
            {
                if (!mersisNo.StartsWith("0"))
                {
                    return false;

                }
                else
                {
                    vergiNo = mersisNo.Substring(1, 10);
                    if (!IsTaxNumber(vergiNo))
                    {
                        return false;
                    }
                    else
                    {
                        if (mersisNo.EndsWith("00015") || mersisNo.EndsWith("00016") || mersisNo.EndsWith("00017"))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
