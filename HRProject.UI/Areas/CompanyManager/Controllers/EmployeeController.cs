using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using HRProject.UI.Helper;
using HRProject.UI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Claims;

namespace HRProject.UI.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager"), Authorize(Roles = "CompanyManager")]
    public class EmployeeController : Controller
    {
        string baseURL = "https://localhost:7127";
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment environment;

        public EmployeeController(IMapper mapper, IWebHostEnvironment environment)
        {
            _mapper = mapper;
            this.environment = environment;
        }

        public async Task<IActionResult> GetEmployeeList()
        {
            List<Employee> employees = new List<Employee>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Employee/GetAllEmployees"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    employees = JsonConvert.DeserializeObject<List<Employee>>(apiCevap);
                }
            }

            ViewBag.mssg = TempData["mssg"] as string;
            return View(employees);
        }


        public static List<Company> companies;
        public static List<Job> jobs;
        public static CompanyManagerEntity employees;
        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginIdClaim = currentUser.FindFirst("ID");

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
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/CompanyManager/GetCompanyManagerById/{loginIdClaim.Value}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    employees = JsonConvert.DeserializeObject<List<CompanyManagerEntity>>(apiCevap)[0];
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
        public async Task<IActionResult> CreateEmployee(Employee employee, List<IFormFile> files)
        {
            if (employee.FirstName != null && employee.LastName != null)
            {
                employee.EmailAddress = employee.CreateEmail(employee.FirstName, employee.MiddleName, employee.LastName);
            }
            employee.Password = PasswordGenerator.GeneratePassword();
            employee.CompanyId = employees.CompanyId;
            if (employee.QuitDate <= DateTime.Now)
            {
                employee.IsActive = false;
            }
            if (files.Count == 0) //Foto seçilemez ise
            {
                if (employee.PhotoURL != "/Uploads/100ad05c_0452_466e_92cb_f2fe1f0755a8.png")
                {
                    employee.PhotoURL = BackUpPhotoURL;
                }
                else
                {
                    employee.PhotoURL = "/Uploads/100ad05c_0452_466e_92cb_f2fe1f0755a8.png";
                }

            }
            else
            {
                string returnedMessaage = Upload.ImageUpload(files, environment, out bool imgresult);

                if (imgresult)
                {
                    employee.PhotoURL = returnedMessaage;
                    BackUpPhotoURL = employee.PhotoURL;
                }
                else
                {
                    employee.PhotoURL = "/Uploads/3b690160_d5f1_4fcf_9712_f6d86d64b9ee.png";
                    ViewBag.PhotoMessage = returnedMessaage;
                    ModelState.AddModelError("", ViewBag.PhotoMessage);

                }
            }
            if (employee.HireDate.HasValue)
            {
                DateTime today = DateTime.Today;
                TimeSpan difference = today - employee.HireDate.Value;
                double yearsDifference = difference.TotalDays / 365.25;

                if (yearsDifference < 1)
                {
                    employee.AnnualDay = 0;
                }
                else if (yearsDifference >= 1 && yearsDifference <= 6)
                {
                    employee.AnnualDay = 14;
                }
                else
                {
                    employee.AnnualDay = 20;
                }
            }
            else
            {
                employee.AnnualDay = 0;
            }

            ViewBag.Jobs = jobs;
            ViewBag.CompanyName = companies;

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{baseURL}/api/Employee/CreateEmployee", content))
                {
                    if (!cevap.IsSuccessStatusCode)
                    {
                        var jsonResponse = await cevap.Content.ReadAsStringAsync();

                        if (!jsonResponse.Contains("The employee already exist"))
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
                            ModelState.AddModelError("ExistingError", "The employee already exist");
                            ViewData["allmasseges"] = ModelState.ToList();
                            return View(employee);
                        }

                        ViewData["allmasseges"] = ModelState.ToList();
                        return View(employee);
                    }

                    if (!ModelState.IsValid)
                    {
                        ViewData["allmasseges"] = ModelState.ToList();
                        return View(employee);
                    }
                }
            }
            string subject = "Hesap Oluşturuldu";
            string body = $"Hello {employee.FirstName}, we are very happy that you have joined us. We hope you have a lot of fun in our Galaxy application. We have created an email address and password for you. You can log in with this data by clicking the link below. Have fun. Have fun.\n\nYour Email Address : {employee.EmailAddress}\nYour Password : {employee.Password} \n\n Login Link : https://hrprojectui20230605130009.azurewebsites.net/ ";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("galaxyhrsystem@outlook.com");
                mail.To.Add(employee.EmailAddress);
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
            return RedirectToAction("GetEmployeeList", "CompanyManager");
        }
    }
}
