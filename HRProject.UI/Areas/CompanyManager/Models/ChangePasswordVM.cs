using System.ComponentModel.DataAnnotations;

namespace HRProject.UI.Areas.CompanyManager.Models
{
    public class ChangePasswordVM
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="Old Password cannot be empty")]
        public string? OldPassword { get; set; }
        [Required(ErrorMessage = "New Password cannot be empty")]

        public string? NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password cannot be empty")]

        public string? ConfirmPassword { get; set;}
    }
}
