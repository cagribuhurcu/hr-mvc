using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using HRProject.Entities.Validation;
using HRProject.UI.Areas.CompanyManager.Models;
using HRProject.UI.Models;
using HRProject.UI.Models.Entitites;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace HRProject.UI.Controllers
{
    public class HomeController : Controller
    {

        string baseURL = "https://localhost:7127";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //https://localhost:7270/api/User/Login?email=ba%40ba.com&password=12345.A
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            User logged = new User();
            using (var httpClient = new HttpClient())
            {
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/Account/Login?email={loginVM.EmailAddress}&password={loginVM.Password}"))
                {
                    if (!answ.IsSuccessStatusCode)
                    {
                        string errorMessage = "";
                        if (loginVM.EmailAddress == null || loginVM.Password == null)
                        {
                            errorMessage = "Email address or password cannot be empty";
                        }
                        else
                        {
                            errorMessage = await answ.Content.ReadAsStringAsync();
                        }
                        ModelState.AddModelError("Error", errorMessage);
                        return View(loginVM);
                    }
                    else
                    {
                        string apiResult = await answ.Content.ReadAsStringAsync();
                        logged = JsonConvert.DeserializeObject<User>(apiResult);
                    }
                }

            }
            if (logged != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim ("ID",logged.ID.ToString()),
                    new Claim("PhotoUrl",logged.PhotoURL),
                    new Claim(ClaimTypes.Name,logged.FirstName+" "+logged.MiddleName),
                    new Claim(ClaimTypes.Surname,logged.LastName+" "+logged.SecondLastName),
                    new Claim(ClaimTypes.Email,logged.EmailAddress),
                    new Claim(ClaimTypes.Role,logged.Role.ToString()),
                    new Claim("IsPasswordChange",logged.IsPasswordChange.ToString()),
                    new Claim("IdentificationNumber",logged.IdentificationNumber),
                };
                var userIdentity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
            }
            else
            {
                return View(loginVM);
            }
            if (logged.IsPasswordChange == true)
            {
                switch (logged.Role)
                {
                    case Entities.Enums.Roles.CompanyManager:
                        return RedirectToAction("Index", "CompanyManager", new { Area = "CompanyManager" });
                    case Entities.Enums.Roles.SiteManager:
                        return RedirectToAction("Index", "SiteManagement", new { Area = "SiteManagement" });
                    case Entities.Enums.Roles.Employee:
                        return RedirectToAction("Index", "Employment", new { Area = "Employment" }); //Düzeltilecek
                    default:
                        return View(loginVM);
                }
            }
            else
            {

                return RedirectToAction("FirstPasswordChange", "Home");

            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Home", new { Area = "" });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public static User loggedUser;
        [HttpGet]
        public async Task<IActionResult> FirstPasswordChange()
        {
            
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginIdentificationNumberClaim = currentUser.FindFirst("IdentificationNumber");
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Account/GetUserByIdentificationNumber/{loginIdentificationNumberClaim.Value.ToString()}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    loggedUser = JsonConvert.DeserializeObject<User>(apiCevap);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FirstPasswordChange(FirstChangeVM firstChangeVM)
        {
            //CompanyManagerEntity oldPasscompany = new CompanyManagerEntity();
            //oldPasscompany.Password = loggedUser.Password;
            if (firstChangeVM.NewPassword == firstChangeVM.ConfirmPassword)
            {
                if (firstChangeVM.OldPassword == loggedUser.Password)
                {
                    loggedUser.Password = firstChangeVM.NewPassword;
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(loggedUser), Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PutAsync($"{baseURL}/api/Account/FirstPasswordChange/{loggedUser.IdentificationNumber}", content))
                        {

                            if (!response.IsSuccessStatusCode)
                            {
                                //loggedUser.Password = oldPasscompany.Password;
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
                                return View();
                            }

                            if (!ModelState.IsValid)
                            {
                                ViewData["allmasseges"] = ModelState.ToList();
                                return View();
                            }
                        }
                    }
                    
                    TempData["mssg"] = "Update successful!";
                    return RedirectToAction("Logout");
                }
                else
                {
                    ModelState.AddModelError("", "Please enter your old password correctly");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "New Password and confirm passwords are not equal.");
                return View();
            }
        }
    }
}