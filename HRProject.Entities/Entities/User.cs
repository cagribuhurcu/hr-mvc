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
        public string? EmailAddress { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Departments Department { get; set; }

        [ForeignKey("Job")]
        public int JobID { get; set; }
        public Job? Job { get; set; }
        public string? PhotoURL { get; set; } = "/Uploads/ef2fefcf_0dbb_4239_8b28_7f87983acf87.jpeg";
        public Roles Role { get; set; }

        public string Password { get; set; }

        //ConvertToEnglish Method

        public string CreateEmail(string firstname,string lastname)
        {
            return $"{ConvertToEnglish(firstname).ToLower()}.{ConvertToEnglish(lastname).ToLower()}@bilgeadam.com";
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
