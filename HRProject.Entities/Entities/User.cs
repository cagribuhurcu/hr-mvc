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

        
        public string? PhotoURL { get; set; } = "/Uploads/ef2fefcf_0dbb_4239_8b28_7f87983acf87.jpeg";
        public Roles Role { get; set; }

        public string? Password { get; set; }
    }
}
