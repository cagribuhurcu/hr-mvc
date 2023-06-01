using HRProject.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRProject.UI.Areas.SiteManagement.Controllers
{
    public class SiteManagementController : Controller
    {
        [Area("SiteManagement") /*Authorize(Roles = "Admin")*/]
        public IActionResult Index()
        {
            return View();
        }
    }
}
