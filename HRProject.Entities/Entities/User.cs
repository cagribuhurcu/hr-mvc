using HRProject.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRProject.Entities.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string BirthPlace { get; set; }
        public DateTime BirthDate { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? QuitDate { get; set; }
        public string EmailAddress
        {
            get { return $"{ConvertToEnglish(FirstName).ToLower()}.{ConvertToEnglish(LastName).ToLower()}@bilgeadam.com"; }
        }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Departments Department { get; set; }

        [ForeignKey("Job")]
        public int JobID { get; set; }
        public Job? Job { get; set; }
        public string? PhotoURL { get; set; }
        public Roles Role { get; set; }

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
