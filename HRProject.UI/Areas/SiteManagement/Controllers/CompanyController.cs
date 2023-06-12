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
        string baseURL = "https://hrprojectapi20230605125226.azurewebsites.net";
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
            ViewBag.BaseLogoUrl = "/Uploads/ef2fefcf_0dbb_4239_8b28_7f87983acf87.jpeg";
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
                if(company.LogoURL!= "/Uploads/ef2fefcf_0dbb_4239_8b28_7f87983acf87.jpeg")
                {
                    company.LogoURL = BackUpLogoURL;
                }
                else
                {
                    company.LogoURL = "/Uploads/ef2fefcf_0dbb_4239_8b28_7f87983acf87.jpeg";
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
                    company.LogoURL = "/Uploads/ef2fefcf_0dbb_4239_8b28_7f87983acf87.jpeg";
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
                    company.EmailAddress = company.CreateEmail(company.CompanyName);
                    
                    if (!ModelState.IsValid)
                    {
                        ViewData["allmasseges"] = ModelState.ToList();
                        return View(company);
                    }
                }
            }
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
                    company.LogoURL = "/Uploads/ef2fefcf_0dbb_4239_8b28_7f87983acf87.jpeg";
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
                    company.EmailAddress = company.CreateEmail(company.CompanyName);

                    if (!ModelState.IsValid)
                    {
                        ViewData["allmasseges"] = ModelState.ToList();
                        return View(company);
                    }
                }
            }
            
            return RedirectToAction("Index");
        }
    }
}
