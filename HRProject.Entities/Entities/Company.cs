using HRProject.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Entities
{
    public class Company:BaseEntity
    {
        public string CompanyName { get; set; }

        public Titles Title { get; set; }

        public string MERSISNo { get; set; }
        public string TaxNumber { get; set; }
        public string TaxAdministration { get; set; }

        public string? LogoURL { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string EmailAddress
        {
            get { return $"info@{ConvertToEnglish(CompanyName).ToLower()}.com"; }
        }

        public int TotalEmployees { get; set; }

        public DateTime FoundationDate { get; set; }
        public DateTime ContractStartDate { get; set; }

        public DateTime ContractEndDate
        {
            get
            {
                return ContractEndDate;
            }
            set
            {
                if (ContractEndDate < DateTime.Now)
                {
                    IsActive = false;
                }
                else
                {
                    IsActive = true;
                }
            }
        }

        //ConvertToEnglish Method
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
