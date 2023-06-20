using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HRProject.Entities.Enums
{
    public enum Currency
    {
        [Display(Name = "Turkish Lira")]
        TurkishLira =1,
        Dolar=2,
        Euro=3,
    }
}
