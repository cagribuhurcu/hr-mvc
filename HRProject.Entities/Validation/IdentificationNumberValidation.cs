using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Validation
{
    public static class IdentificationNumberValidation
    {
        public static bool IdentificationNumberVerify(string? value)
        {
            int toplam = 0;
            int ciftTop = 0;
            int tekTop = 0;

            if (value != null)
            {
                string id = value.ToString();
                if (id.Length != 11 || id[0] == '0')
                    return false;

                for (int i = 0; i <= 9; i++)
                {
                    toplam += Convert.ToInt32(id[i].ToString());

                    if (i % 2 == 0) //Çiftinci indexde ise (tekinci handeyiz demektir)
                    {
                        tekTop += Convert.ToInt32(id[i].ToString());
                    }
                    else if (i % 2 != 0 && i <= 7) //Tekinci indexde ise (çiftinci handeyiz demektir)
                    {
                        ciftTop += Convert.ToInt32(id[i].ToString());
                    }
                }
                //10. hanedeki değerin algoritması
                if (((tekTop * 7) - ciftTop) % 10 != Convert.ToInt32(id[9].ToString()))
                {
                    return false;
                }
                //11. hanedeki değerin algoritması
                if (toplam % 10 != Convert.ToInt32(id[10].ToString()))
                {
                    return false;
                }

                return true;
            }
            return false;
        }
    }
}
