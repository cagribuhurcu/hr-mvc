using HRProject.Entities.Enums;

namespace HRProject.UI.Models.Entitites
{
    public class UserVM
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Roles UserRole { get; set; }
    }
}
