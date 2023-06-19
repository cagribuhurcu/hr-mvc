using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using HRProject.UI.Areas.SiteManagement.Models;
using HRProject.UI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Policy;
using System.Text;
using static System.Net.WebRequestMethods;

namespace HRProject.UI.Areas.SiteManagement.Controllers
{
    [Area("SiteManagement"), Authorize(Roles = "SiteManager")]
    public class CompanyController : Controller
    {
        string baseURL = "https://localhost:7127";
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment environment;

        public CompanyController(IMapper mapper, IWebHostEnvironment environment)
        {
            _mapper = mapper;
            this.environment = environment;
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

            ViewBag.mssg = TempData["mssg"] as string;

            return View(companies);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            Company company = new Company();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Company/GetCompanyById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    company = JsonConvert.DeserializeObject<Company>(apiCevap);
                }
            }
            return Json(company);
        }

        [HttpGet]
        public IActionResult CreateCompany()
        {
            ViewBag.BaseLogoUrl = "/Uploads/3b690160_d5f1_4fcf_9712_f6d86d64b9ee.png.png";
            return View();
        }
        public static string BackUpLogoURL;

        [HttpPost]
        public async Task<IActionResult> CreateCompany(Company company, List<IFormFile> files)
        {
            if (company.ContractEndDate <= DateTime.Now)
            {
                company.IsActive = false;
            }
            if (files.Count == 0) //Foto seçilemez ise
            {
                if(company.LogoURL!= "/Uploads/3b690160_d5f1_4fcf_9712_f6d86d64b9ee.png.png")
                {
                    company.LogoURL = BackUpLogoURL;
                }
                else
                {
                    company.LogoURL = "/Uploads/3b690160_d5f1_4fcf_9712_f6d86d64b9ee.png.png";
                }
               
            }
            else
            {
                string returnedMessaage = Upload.ImageUpload(files, environment, out bool imgresult);

                if (imgresult)
                {
                    company.LogoURL = returnedMessaage;
                    BackUpLogoURL = company.LogoURL;
                }
                else
                {
                    company.LogoURL = "/Uploads/3b690160_d5f1_4fcf_9712_f6d86d64b9ee.png.png";
                    ViewBag.PhotoMessage = returnedMessaage;
                    ModelState.AddModelError("", ViewBag.PhotoMessage);

                }
            }

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{baseURL}/api/Company/CreateCompany", content))
                {
                    if (!cevap.IsSuccessStatusCode)
                    {
                        var jsonResponse = await cevap.Content.ReadAsStringAsync();
                        
                        if(!jsonResponse.Contains("The company already exists"))
                        {
                            var errorResponseAll = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                            string errorMessage = "";
                            if (errorResponseAll.errors!=null)
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
                        }
                        else
                        {
                            ModelState.AddModelError("ExistingError","The company already exists");
                            ViewData["allmasseges"] = ModelState.ToList();
                            return View(company);
                        }
                        
                         ViewData["allmasseges"] = ModelState.ToList();
                         return View(company);

                    }
                    
                    if (!ModelState.IsValid)
                    {
                        ViewData["allmasseges"] = ModelState.ToList();
                        return View(company);
                    }
                }
            }
            TempData["mssg"] = "Add successful!";
            return RedirectToAction("Index");
        }

        static Company updatedCompany;

        [HttpGet]
        public async Task<IActionResult> UpdateCompany(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Company/GetCompanyById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedCompany = JsonConvert.DeserializeObject<Company>(apiCevap);
                }
            }

            return View(updatedCompany);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCompany(Company company, List<IFormFile> files)
        {
            

            if (files.Count == 0)
            {
                company.LogoURL = updatedCompany.LogoURL;
            }
            else
            {

                string returnedMessaage = Upload.ImageUpload(files, environment, out bool imgresult);

                if (imgresult)
                {
                    company.LogoURL = returnedMessaage;
                }
                else
                {
                    company.LogoURL = "/Uploads/3b690160_d5f1_4fcf_9712_f6d86d64b9ee.png";
                    ViewBag.PhotoMessage = returnedMessaage;
                    ModelState.AddModelError("", ViewBag.PhotoMessage);
                }
            }

            using (var httpClient = new HttpClient())
            {
                updatedCompany = _mapper.Map<Company>(company);

                StringContent content = new StringContent(JsonConvert.SerializeObject(updatedCompany), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{baseURL}/api/Company/UpdateCompany/{updatedCompany.ID}", content))
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
                        return View(company);
                    }

                    if (!ModelState.IsValid)
                    {
                        ViewData["allmasseges"] = ModelState.ToList();
                        return View(company);
                    }
                }
            }

            TempData["mssg"] = "Update successful!";
            return RedirectToAction("Index");
        }
    }
}
