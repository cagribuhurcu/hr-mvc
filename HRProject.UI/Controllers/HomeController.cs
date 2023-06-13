using HRProject.Entities.Entities;
using HRProject.Entities.Validation;
using HRProject.UI.Models;
using HRProject.UI.Models.Entitites;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

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
                using (var answ = await httpClient.GetAsync($"{baseURL}/api/SiteManager/Login?email={loginVM.EmailAddress}&password={loginVM.Password}"))
                {
                    if (!answ.IsSuccessStatusCode)
                    {
                        string errorMessage = "";
                        if (loginVM.EmailAddress==null || loginVM.Password==null)
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
                };
                var userIdentity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
            }
            else
            {
                return View(loginVM);
            }
            switch (logged.Role)
            {
                case Entities.Enums.Roles.CompanyManager:
                    return RedirectToAction("Index", "Home", new { Area = "CompanyManager" }); //Düzeltilecek
                case Entities.Enums.Roles.SiteManager:
                    return RedirectToAction("Index", "SiteManagement", new { Area = "SiteManagement" });
                case Entities.Enums.Roles.Employee:
                    return RedirectToAction("Index", "Home", new { Area = "User" }); //Düzeltilecek
                default:
                    return View(loginVM);
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
    }
}