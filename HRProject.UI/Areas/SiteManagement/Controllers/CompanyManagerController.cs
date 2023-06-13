﻿using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using HRProject.UI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Policy;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using HRProject.UI.Helper;

namespace HRProject.UI.Areas.SiteManagement.Controllers
{
    [Area("SiteManagement"), Authorize(Roles = "SiteManager")]
    public class CompanyManagerController : Controller
    {
        string baseURL = "https://localhost:7127";
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment environment;

        public CompanyManagerController(IMapper mapper, IWebHostEnvironment environment)
        {
            _mapper = mapper;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public static List<Company> companies;
        public static List<Job> jobs;

        [HttpGet]
        public async Task<IActionResult> CreateCompanyManager()
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Company/GetAllCompanies"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    companies = JsonConvert.DeserializeObject<List<Company>>(apiCevap);
                }
            }

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Job/GetAllJobs"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    jobs = JsonConvert.DeserializeObject<List<Job>>(apiCevap);
                }
            }

            ViewBag.Jobs = jobs;
            ViewBag.CompanyName = companies;
            ViewBag.BaseLogoUrl = "/Uploads/3b690160_d5f1_4fcf_9712_f6d86d64b9ee.png";
            return View();
        }

        public static string BackUpPhotoURL;

        [HttpPost]
        public async Task<IActionResult> CreateCompanyManager(CompanyManagerEntity companyManager, List<IFormFile> files)
        {
            companyManager.EmailAddress = companyManager.CreateEmail(companyManager.FirstName, companyManager.LastName);
            companyManager.Password = PasswordGenerator.GeneratePassword();

            if (companyManager.QuitDate <= DateTime.Now)
            {
                companyManager.IsActive = false;
            }
            if (files.Count == 0) //Foto seçilemez ise
            {
                if (companyManager.PhotoURL != "/Uploads/3b690160_d5f1_4fcf_9712_f6d86d64b9ee.png")
                {
                    companyManager.PhotoURL = BackUpPhotoURL;
                }
                else
                {
                    companyManager.PhotoURL = "/Uploads/3b690160_d5f1_4fcf_9712_f6d86d64b9ee.png";
                }

            }
            else
            {
                string returnedMessaage = Upload.ImageUpload(files, environment, out bool imgresult);

                if (imgresult)
                {
                    companyManager.PhotoURL = returnedMessaage;
                    BackUpPhotoURL = companyManager.PhotoURL;
                }
                else
                {
                    companyManager.PhotoURL = "/Uploads/3b690160_d5f1_4fcf_9712_f6d86d64b9ee.png";
                    ViewBag.PhotoMessage = returnedMessaage;
                    ModelState.AddModelError("", ViewBag.PhotoMessage);

                }
            }

            ViewBag.Jobs = jobs;
            ViewBag.CompanyName = companies;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(companyManager), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{baseURL}/api/CompanyManager/CreateCompanyManager", content))
                {
                    if (!cevap.IsSuccessStatusCode)
                    {
                        var jsonResponse = await cevap.Content.ReadAsStringAsync();

                        if (!jsonResponse.Contains("The company manager already exists"))
                        {
                            var errorResponseAll = JsonConvert.DeserializeObject<dynamic>(jsonResponse);//
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
                        }
                        else
                        {
                            ModelState.AddModelError("ExistingError", "The company manager already exists");
                            ViewData["allmasseges"] = ModelState.ToList();
                            return View(companyManager);
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
            string subject = "Hesap Oluşturuldu";
            string body = $"Merhaba {companyManager.FirstName} senin için bir hesap oluşturduk \n Mailin = {companyManager.EmailAddress} ve Şifren : {companyManager.Password}";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("galaxyhrsystem@outlook.com");
                mail.To.Add(companyManager.EmailAddress);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;



                using (SmtpClient smtp = new SmtpClient("smtp.office365.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("galaxyhrsystem@outlook.com", "123ASD123");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

            return RedirectToAction("Index","SiteManagement");
        }
    }
}
