using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using HRProject.UI.Areas.SiteManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HRProject.UI.Areas.SiteManagement.Controllers
{
    [Area("SiteManagement") /*Authorize(Roles = "Admin")*/]
    public class SiteManagementController : Controller
    {
        private readonly IMapper _mapper;
        string url = "https://localhost:7127";
        
        public SiteManagementController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<User> users = new List<User>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{url}/api/User/GetAllUsers"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<List<User>>(apiCevap);
                }
            }

            List<UserVM> userVMs = _mapper.Map<List<UserVM>>(users);

            return View(userVMs);
        }
    }
}
