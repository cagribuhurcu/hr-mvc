using HRProject.Entities.Entities;
using HRProject.Entities.Enums;

namespace HRProject.UI.Areas.SiteManagement.Models
{
    public class UserVM
    {
        public UserVM(User user)
        {
            FullName = $"{user.FirstName} {user.MiddleName} {user.LastName} {user.SecondLastName}";
            EmailAddress = user.Email;
            PhoneNumber = user.PhoneNumber;
            Address = user.Address;
            Job = user.Job;
            Department = user.Department;
        }

        public string PhotoURL { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public Job Job { get; set; }
        public Departments Department { get; set; }
    }
}
