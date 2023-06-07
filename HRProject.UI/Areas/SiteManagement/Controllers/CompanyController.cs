using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using HRProject.UI.Areas.SiteManagement.Models;
using HRProject.UI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Security.Policy;
using System.Text;

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
            return View(companies);
        }

        [HttpGet]
        public IActionResult CreateCompany()
        {
            return View();
        }

        static Company createCompany;

        [HttpPost]
        public async Task<IActionResult> CreateCompany(Company company, List<IFormFile> files)
        {
            company.IsActive = true;
            if (files.Count == 0) //Foto seçilemez ise
            {
                company.LogoURL = createCompany.LogoURL;
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
                StringContent content = new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{baseURL}/api/Company/CreateCompany", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
