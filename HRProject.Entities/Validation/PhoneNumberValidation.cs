using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Validation
{
    public static class PhoneNumberValidation
    {
        public static bool BeAllDigits(string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;

            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                    return false;
            }

            return true;
        }
    }
}
