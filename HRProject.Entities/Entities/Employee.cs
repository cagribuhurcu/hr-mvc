using HRProject.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Entities
{
    public class Employee : User
    {
        [ForeignKey("Job")]
        public int? JobID { get; set; }
        public Job? Job { get; set; }
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }
        public decimal? Salary { get; set; }

        public Gender? Gender { get; set; }


        //Create email for employee
        public string CreateEmail(string firstname, string middlename, string lastname)
        {
            if (middlename != null)
            {
                return $"{ConvertToEnglish(firstname).ToLower()}{ConvertToEnglish(middlename).ToLower()}.{ConvertToEnglish(lastname).ToLower()}@bilgeadamboost.com";
            }
            else
            {
                return $"{ConvertToEnglish(firstname).ToLower()}.{ConvertToEnglish(lastname).ToLower()}@bilgeadamboost.com";
            }

        }

        private string ConvertToEnglish(string text)
        {
            StringBuilder convertedText = new StringBuilder();
            foreach (char c in text)
            {
                convertedText.Append(ConvertCharacterToEnglish(c));
            }
            return convertedText.ToString();
        }

        private char ConvertCharacterToEnglish(char c)
        {
            switch (c)
            {
                case 'ı':
                    return 'i';
                case 'I':
                    return 'i';
                case 'İ':
                    return 'I';
                case 'ç':
                    return 'c';
                case 'Ç':
                    return 'C';
                case 'ş':
                    return 's';
                case 'Ş':
                    return 'S';
                case 'ğ':
                    return 'g';
                case 'Ğ':
                    return 'G';
                case 'ü':
                    return 'u';
                case 'Ü':
                    return 'U';
                case 'ö':
                    return 'o';
                case 'Ö':
                    return 'O';
                case 'â':
                case 'Â':
                case 'î':
                case 'Î':
                case 'û':
                case 'Û':
                    return 'a';
                default:
                    return c;
            }
        }

    }
}
