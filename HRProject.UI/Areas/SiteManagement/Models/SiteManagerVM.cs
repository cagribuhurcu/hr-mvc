using HRProject.Entities.Entities;
using HRProject.Entities.Enums;

namespace HRProject.UI.Areas.SiteManagement.Models
{
    public class SiteManagerVM
    {
        public int ID { get; set; }
        public string? PhotoURL { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public Job Job { get; set; }
        public int JobID { get; set; }
        public Departments Department { get; set; }
    }
}
