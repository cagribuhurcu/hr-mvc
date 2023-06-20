using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace HRProject.UI.Areas.Employment.Controllers
{
    [Area("Employment"), Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment environment;
        string baseURL = "https://localhost:7127";

        public EmployeeController(IMapper mapper, IWebHostEnvironment environment)
        {
            _mapper = mapper;
            this.environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginIdClaim = currentUser.FindFirst("ID");

            Employee employee = new Employee();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Employee/GetEmployeeById/{loginIdClaim.Value}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<List<Employee>>(apiCevap)[0];
                }
            }
            var newLoginClaim = new Claim("PhotoUrl", employee.PhotoURL);

            var Employee = HttpContext.User.Identity as ClaimsIdentity;
            var loginClaim = Employee.FindFirst("PhotoUrl");
            if (loginClaim != null)
            {
                Employee.RemoveClaim(loginClaim);
                Employee.AddClaim(newLoginClaim);
            }
            var newPrincipal = new ClaimsPrincipal(Employee);
            await HttpContext.SignInAsync(newPrincipal);
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            Employee employee = new Employee();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Employee/GetEmployeeById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<List<Employee>>(apiCevap)[0];
                }
            }
            return Json(employee);
        }
        [HttpGet]
        public async Task<IActionResult> PermissionList(int id)
        {
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginIdClaim = currentUser.FindFirst("ID");

            List<EmployeePermission> employeePermissions=new List<EmployeePermission>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Permission/GetAllPermissionbyEmployeeId/{loginIdClaim.Value}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    employeePermissions = JsonConvert.DeserializeObject<List<EmployeePermission>>(apiCevap);
                }
            }
            return View(employeePermissions);
        }
    }
}
