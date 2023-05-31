using HRProject.Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace HRProject.Entities.Entities
{
    public class User : IdentityUser
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
            get { return $"{FirstName}.{LastName}@bilgeadam.com"; }
        }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public Departments Department { get; set; }
        public Job Job { get; set; }

    }
}
