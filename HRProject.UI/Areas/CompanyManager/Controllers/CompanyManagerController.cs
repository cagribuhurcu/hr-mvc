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
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginIdClaim = currentUser.FindFirst("ID");

            CompanyManagerEntity companyManagers = new CompanyManagerEntity();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/CompanyManager/GetCompanyManagerById/{loginIdClaim.Value}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    companyManagers = JsonConvert.DeserializeObject<List<CompanyManagerEntity>>(apiCevap)[0];
                }
            }
            var newLoginClaim = new Claim("PhotoUrl", companyManagers.PhotoURL);

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

        //Update

        static CompanyManagerEntity updatedCompanyManager;
        public static List<Company> companiess;
        public static List<Job> jobss;

        [HttpGet]
        public async Task<IActionResult> UpdateCompanyManager(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Company/GetAllCompanies"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    companiess = JsonConvert.DeserializeObject<List<Company>>(apiCevap);
                }
            }

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/CompanyManager/GetCompanyManagerById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedCompanyManager = JsonConvert.DeserializeObject<List<CompanyManagerEntity>>(apiCevap)[0];
                }
            }

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Job/GetAllJobs"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    jobss = JsonConvert.DeserializeObject<List<Job>>(apiCevap);
                }
            }

            ViewBag.Jobss = jobss;
            ViewBag.CompanyNamee = companiess;
            return View(updatedCompanyManager);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCompanyManager(CompanyManagerEntity companyManager, List<IFormFile> files)
        {
            companyManager.Password = updatedCompanyManager.Password;

            if (files.Count == 0)
            {
                companyManager.PhotoURL = updatedCompanyManager.PhotoURL;
            }
            else
            {

                string returnedMessaage = Upload.ImageUpload(files, environment, out bool imgresult);

                if (imgresult)
                {
                    companyManager.PhotoURL = returnedMessaage;
                }
                else
                {
                    companyManager.PhotoURL = "/Uploads/3b690160_d5f1_4fcf_9712_f6d86d64b9ee.png";
                    ViewBag.PhotoMessage = returnedMessaage;
                    ModelState.AddModelError("", ViewBag.PhotoMessage);
                }
            }

            using (var httpClient = new HttpClient())
            {
                updatedCompanyManager = _mapper.Map<CompanyManagerEntity>(companyManager);

                StringContent content = new StringContent(JsonConvert.SerializeObject(updatedCompanyManager), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{baseURL}/api/CompanyManager/UpdateCompanyManager/{updatedCompanyManager.ID}", content))
                {

                    if (!response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var errorResponseAll = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                        string errorMessage = "";
                        if (errorResponseAll.errors != null)
                        {
                            var errors = errorResponseAll.errors;
                            foreach (var error in errors)
                            {
                                var errorMessages = (JArray)error.Value;
                                foreach (var errorMessageToken in errorMessages)
                                {
                                    errorMessage = errorMessageToken.ToString() + "\n";
                                    ModelState.AddModelError("", errorMessage);
                                }
                            }
                        }

                        ViewData["allmasseges"] = ModelState.ToList();
                        return View(companyManager);
                    }

                    if (!ModelState.IsValid)
                    {
                        ViewData["allmasseges"] = ModelState.ToList();
                        return View(companyManager);
                    }
                }
            }

            TempData["mssg"] = "Update successful!";
            return RedirectToAction("Index");
        }
    }
}
