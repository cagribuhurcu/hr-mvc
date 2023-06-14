using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using HRProject.UI.Areas.SiteManagement.Models;
using HRProject.UI.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace HRProject.UI.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager"), Authorize(Roles = "CompanyManager")]
    public class CompanyManagerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment environment;
        string baseURL = "https://localhost:7127";

        public CompanyManagerController(IMapper mapper, IWebHostEnvironment environment)
        {
            _mapper = mapper;
            this.environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            List<CompanyManagerEntity> companyManagers = new List<CompanyManagerEntity>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/CompanyManager/GetAllCompanyManagers"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    companyManagers = JsonConvert.DeserializeObject<List<CompanyManagerEntity>>(apiCevap);
                }
            }
            var newLoginClaim = new Claim("PhotoUrl", companyManagers[0].PhotoURL);

            var currentCompanyManager = HttpContext.User.Identity as ClaimsIdentity;
            var loginClaim = currentCompanyManager.FindFirst("PhotoUrl");
            if (loginClaim != null)
            {
                currentCompanyManager.RemoveClaim(loginClaim);
                currentCompanyManager.AddClaim(newLoginClaim);
            }
            var newPrincipal = new ClaimsPrincipal(currentCompanyManager);
            await HttpContext.SignInAsync(newPrincipal);
            return View(companyManagers);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            CompanyManagerEntity companyManager = new CompanyManagerEntity();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/CompanyManager/GetCompanyManagerById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    companyManager = JsonConvert.DeserializeObject<List<CompanyManagerEntity>>(apiCevap)[0];
                }
            }
            return Json(companyManager);
        }
    }
}
