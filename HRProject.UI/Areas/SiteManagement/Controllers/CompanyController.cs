using AutoMapper;
using HRProject.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRProject.UI.Areas.SiteManagement.Controllers
{
    [Area("SiteManagement"), Authorize(Roles = "SiteManager")]
    public class CompanyController : Controller
    {
        string baseURL = "https://hrprojectapi20230605125226.azurewebsites.net";
        private readonly IMapper _mapper;

        public CompanyController(IMapper mapper)
        {
            _mapper = mapper;
          
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
