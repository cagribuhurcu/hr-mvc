using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Enums
{
    public enum Departments
    {
        [Description("Human Resources")]
        HumanResources =1,//İnsan Kaynakları
        Marketing=2,//Pazarlama
        Finance=3,//Finans
        Sales=4,//Satış
        [Description("Information Systems")]
        InformationSystems =5//Bilişim Sistemlerii
    }
}
