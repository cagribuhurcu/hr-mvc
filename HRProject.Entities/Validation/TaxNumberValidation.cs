using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Validation
{
    public static class TaxNumberValidation
    {
        public static bool IsTaxNumber(string txn)
        {
            int tmp;
            int sum = 0;
            if (txn != null && txn.Length == 10 && IsInt(txn))
            {
                int lastDigit = int.Parse(txn[9].ToString());
                for (int i = 0; i < 9; i++)
                {
                    int digit = int.Parse(txn[i].ToString());
                    tmp = (digit + 10 - (i + 1)) % 10;
                    sum = (int)((tmp == 9) ? sum + tmp : sum + ((tmp * (Math.Pow(2, 10 - (i + 1)))) % 9));
                }
                return lastDigit == (10 - (sum % 10)) % 10;
            }
            return false;
        }

        private static bool IsInt(string s)
        {
            for (int a = 0; a < s.Length; a++)
            {
                if (a == 0 && s[a] == '-') continue;
                if (!char.IsDigit(s[a])) return false;
            }
            return true;
        }
    }
}
