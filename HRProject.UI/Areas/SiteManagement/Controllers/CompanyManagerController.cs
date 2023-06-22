using AutoMapper;
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
        string baseURL = "https://hrprojectapi20230623002753.azurewebsites.net";
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment environment;

        public CompanyManagerController(IMapper mapper, IWebHostEnvironment environment)
        {
            _mapper = mapper;
            this.environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            List<CompanyManagerEntity> companyManager = new List<CompanyManagerEntity>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/CompanyManager/GetAllCompanyManagers"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    companyManager = JsonConvert.DeserializeObject<List<CompanyManagerEntity>>(apiCevap);
                }
            }

            ViewBag.mssg = TempData["mssg"] as string;
            return View(companyManager);
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
            ViewBag.BaseLogoUrl = "/Uploads/100ad05c_0452_466e_92cb_f2fe1f0755a8.png";
            return View();
        }

        public static string BackUpPhotoURL;

        [HttpPost]
        public async Task<IActionResult> CreateCompanyManager(CompanyManagerEntity companyManager, List<IFormFile> files)
        {
            if(companyManager.FirstName!=null && companyManager.LastName != null)
            {
                companyManager.EmailAddress = companyManager.CreateEmail(companyManager.FirstName,companyManager.MiddleName, companyManager.LastName);
            }
            companyManager.Password = PasswordGenerator.GeneratePassword();

            if (companyManager.QuitDate <= DateTime.Now)
            {
                companyManager.IsActive = false;
            }
            if (files.Count == 0) //Foto seçilemez ise
            {
                if (companyManager.PhotoURL != "/Uploads/100ad05c_0452_466e_92cb_f2fe1f0755a8.png")
                {
                    companyManager.PhotoURL = BackUpPhotoURL;
                }
                else
                {
                    companyManager.PhotoURL = "/Uploads/100ad05c_0452_466e_92cb_f2fe1f0755a8.png";
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
            string body = $"Hello {companyManager.FirstName}, we are very happy that you have joined us. We hope you have a lot of fun in our Galaxy application. We have created an email address and password for you. You can log in with this data by clicking the link below. Have fun. Have fun.\n\nYour Email Address : {companyManager.EmailAddress}\nYour Password : {companyManager.Password} \n\n Login Link : https://hrprojectui20230605130009.azurewebsites.net/ ";

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

            TempData["mssg"] = "Add successful!";
            return RedirectToAction("Index","SiteManagement");
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
            ViewBag.Jobss = jobss;
            ViewBag.CompanyNamee = companiess;
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
