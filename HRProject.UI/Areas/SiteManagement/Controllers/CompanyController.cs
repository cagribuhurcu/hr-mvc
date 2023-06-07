using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HRProject.UI.Areas.SiteManagement.Controllers
{
    [Area("SiteManagement"), Authorize(Roles = "SiteManager")]
    public class CompanyController : Controller
    {
        string baseURL = "https://localhost:7127";
        private readonly IMapper _mapper;

        public CompanyController(IMapper mapper)
        {
            _mapper = mapper;
          
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Company> companies = new List<Company>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Company/GetAllCompanies"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    companies = JsonConvert.DeserializeObject<List<Company>>(apiCevap);
                }
            }
            return View(companies);
        }
    }
}
