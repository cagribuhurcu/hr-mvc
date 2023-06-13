using HRProject.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Entities
{
    public class Employee:BaseEntity
    {
        public string? PhotoURL { get; set; }

        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string BirthPlace { get; set; }
        public DateTime BirthDate { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime HireDate { get; set; }

        private DateTime? quitDate;
        public DateTime? QuitDate
        {
            get
            {
                return quitDate;
            }
            set
            {
                quitDate = value; IsActive = !quitDate.HasValue;
            }
        }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int JobID { get; set; }
        public Job? Job { get; set; }

        public Departments Department { get; set; }

        public string? EmailAddress { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public decimal Salary { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }

        public string CreateEmail(string firstname, string lastname)
        {
            return $"{ConvertToEnglish(firstname).ToLower()}.{ConvertToEnglish(lastname).ToLower()}@bilgeadamboost.com";
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
