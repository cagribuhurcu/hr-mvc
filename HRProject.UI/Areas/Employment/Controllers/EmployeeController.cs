using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using HRProject.UI.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;

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

            List<EmployeePermission> employeePermissions = new List<EmployeePermission>();
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

        public static List<Permission> permissionNames;
        public static Employee updatedEmployee;

        public static int employeeId;

        [HttpGet]
        public async Task<IActionResult> CreatePermissionforEmployee()
        {
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginIdClaim = currentUser.FindFirst("ID");

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Permission/GetAllPermission"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    permissionNames = JsonConvert.DeserializeObject<List<Permission>>(apiCevap);
                }
            }

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Employee/GetEmployeeById/{loginIdClaim.Value}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedEmployee = JsonConvert.DeserializeObject<List<Employee>>(apiCevap)[0];
                }
            }
            ViewBag.PermissionName = permissionNames;
            ViewBag.EmployeeId = loginIdClaim.Value;
            ViewBag.Gender = updatedEmployee.Gender;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePermissionforEmployee(EmployeePermission employeePermission)
        {
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginIdClaim = currentUser.FindFirst("ID");

            List<EmployeePermission> employeePermissions = new List<EmployeePermission>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Permission/GetAllPermissionbyEmployeeId/{loginIdClaim.Value}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    employeePermissions = JsonConvert.DeserializeObject<List<EmployeePermission>>(apiCevap);
                }
            }

            if (employeePermissions.Any(a => a.PermissionState == Status.Pending))
            {
                ViewBag.message = "You already have a pending request. You must wait for it to be approved first. Or you can contact your company manager.";
                ViewBag.PermissionName = permissionNames;
                ViewBag.EmployeeId = loginIdClaim.Value;
                ViewBag.Gender = updatedEmployee.Gender;
                return View();
            }
            else if (employeePermissions.Any(a => a.PermissionState == Status.Confirmed))
            {
                var foundEmp = employeePermissions.FirstOrDefault(x =>
                {
                    bool isStartDateInRange = x.StartDate <= employeePermission.StartDate && employeePermission.StartDate <= x.EndDate;
                    bool isEndDateInRange = x.StartDate <= employeePermission.EndDate && employeePermission.EndDate <= x.EndDate;
                    bool isRangeWithinX = employeePermission.StartDate <= x.StartDate && x.EndDate <= employeePermission.EndDate;

                    return isStartDateInRange || isEndDateInRange || isRangeWithinX;
                });

                if (foundEmp != null)
                {
                    ViewBag.message = "You already have a request on the same date.";
                    ViewBag.PermissionName = permissionNames;
                    ViewBag.EmployeeId = loginIdClaim.Value;
                    ViewBag.Gender = updatedEmployee.Gender;
                    return View();
                }
            }
            if (employeePermission.PermissionId == 7)
            {
                DateTime startDate = employeePermission.StartDate.Value;
                DateTime endDate = employeePermission.EndDate.Value;
                int differenceInDays = (int)(endDate - startDate).TotalDays;
                updatedEmployee.AnnualDay -= differenceInDays;

                if (updatedEmployee.AnnualDay < 0)
                {
                    ViewBag.message = "Çalış biraz çok gezdin";
                    ViewBag.PermissionName = permissionNames;
                    ViewBag.EmployeeId = loginIdClaim.Value;
                    ViewBag.Gender = updatedEmployee.Gender;
                    return View();
                }
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(updatedEmployee), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PutAsync($"{baseURL}/api/Employee/UpdateAnnualDay/{updatedEmployee.ID}", content))
                    {
                        {
                            string apiCevap = await response.Content.ReadAsStringAsync();
                        }
                    }
                }

            }
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(employeePermission), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{baseURL}/api/Permission/CreatePermissionforEmployee", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("PermissionList");

        }

        public static EmployeePermission employeePermission1;
        [HttpGet]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginIdClaim = currentUser.FindFirst("ID");

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Permission/GetEmployeePermissionbyId/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    employeePermission1 = JsonConvert.DeserializeObject<EmployeePermission>(apiCevap);
                }
            }
            if (employeePermission1.PermissionId == 7)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Employee/GetEmployeeById/{loginIdClaim.Value}"))
                    {
                        string apiCevap = await cevap.Content.ReadAsStringAsync();
                        updatedEmployee = JsonConvert.DeserializeObject<List<Employee>>(apiCevap)[0];
                    }
                }
                DateTime startDate = employeePermission1.StartDate.Value;
                DateTime endDate = employeePermission1.EndDate.Value;
                int differenceInDays = (int)(endDate - startDate).TotalDays;
                updatedEmployee.AnnualDay += differenceInDays;

                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(updatedEmployee), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PutAsync($"{baseURL}/api/Employee/UpdateAnnualDay/{updatedEmployee.ID}", content))
                    {
                        {
                            string apiCevap = await response.Content.ReadAsStringAsync();
                        }
                    }
                }

            }
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{baseURL}/api/Permission/DeletePermission/{id}"))
                {

                }
            }
            return RedirectToAction("PermissionList");

        }

        [HttpGet]
        public async Task<IActionResult> DeleteAdvancePayment(int id)
        { 
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{baseURL}/api/AdvancePayment/DeleteAdvancePayment/{id}"))
                {
                    
                }
            }
            return RedirectToAction("AdvancePaymentList");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{baseURL}/api/Expense/DeleteExpense/{id}"))
                {

                }
            }
            return RedirectToAction("ExpenseList");

        }

        ////////////////////////////////////////

        [HttpGet]
        public async Task<IActionResult> AdvancePaymentList()
        {
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginIdClaim = currentUser.FindFirst("ID");

            List<AdvancePayment> advancePayments = new List<AdvancePayment>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/AdvancePayment/GetAllAdvancePaymentbyEmployeeId/{loginIdClaim.Value}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    advancePayments = JsonConvert.DeserializeObject<List<AdvancePayment>>(apiCevap);
                }
            }
            return View(advancePayments);
        }


        [HttpGet]
        public async Task<IActionResult> CreateAdvancePayment()
        {
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginIdClaim = currentUser.FindFirst("ID");
            ViewBag.EmployeeId = loginIdClaim.Value;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvancePayment(AdvancePayment advancePayment)
        {
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginIdClaim = currentUser.FindFirst("ID");

            List<AdvancePayment> advancePayments = new List<AdvancePayment>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/AdvancePayment/GetAllAdvancePaymentbyEmployeeId/{loginIdClaim.Value}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    advancePayments = JsonConvert.DeserializeObject<List<AdvancePayment>>(apiCevap);
                }
            }

            if (advancePayments.Any(a => a.ApprovalStatus == Status.Pending))
            {
                ViewBag.message = "You already have a pending request. You must wait for it to be approved first. Or you can contact your company manager.";
                ViewBag.EmployeeId = loginIdClaim.Value;
                return View();
            }
            else if (advancePayments.Any(a => a.ApprovalStatus == Status.Confirmed))
            {
                var foundEmp = advancePayments.Where(x => x.RequestDate == advancePayment.RequestDate).FirstOrDefault();
                if (foundEmp != null)
                {
                    ViewBag.message = "You already have a request on the same date.";
                    ViewBag.EmployeeId = loginIdClaim.Value;
                    return View();
                }
            }

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(advancePayment), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{baseURL}/api/AdvancePayment/CreateAdvancePayment", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("AdvancePaymentList");
        }

        /////////////////////---------------------------------


        [HttpGet]
        public async Task<IActionResult> ExpenseList()
        {
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginIdClaim = currentUser.FindFirst("ID");

            List<Expense> expenses = new List<Expense>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Expense/GetAllExpensebyEmployeeId/{loginIdClaim.Value}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    expenses = JsonConvert.DeserializeObject<List<Expense>>(apiCevap);
                }
            }
            return View(expenses);
        }


        [HttpGet]
        public async Task<IActionResult> CreateExpense()
        {
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginIdClaim = currentUser.FindFirst("ID");
            ViewBag.EmployeeId = loginIdClaim.Value;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense(Expense expense, List<IFormFile> files)
        {
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginIdClaim = currentUser.FindFirst("ID");

            List<Expense> expenses = new List<Expense>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Expense/GetAllExpensebyEmployeeId/{loginIdClaim.Value}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    expenses = JsonConvert.DeserializeObject<List<Expense>>(apiCevap);
                }
            }

            if (expenses.Any(a => a.ApprovalStatus == Status.Pending))
            {
                ViewBag.message = "You already have a pending request. You must wait for it to be approved first. Or you can contact your company manager.";
                ViewBag.EmployeeId = loginIdClaim.Value;
                return View();
            }
            else if (expenses.Any(a => a.ApprovalStatus == Status.Confirmed))
            {
                var foundEmp = expenses.Where(x => x.RequestDate == expense.RequestDate).FirstOrDefault();
                if (foundEmp != null)
                {
                    ViewBag.message = "You already have a request on the same date.";
                    ViewBag.EmployeeId = loginIdClaim.Value;
                    return View();
                }
            }


            if (files.Count == 0)
            {
                //expence.FileURL = updatedCompanyManager.PhotoURL;
            }
            else
            {

                string returnedMessaage = UploadFiles.FilesUpload(files, environment, out bool filesult);

                if (filesult)
                {
                    expense.FileURL = returnedMessaage;
                }
                else
                {

                    ViewBag.FileMessage = returnedMessaage;
                    ModelState.AddModelError("", ViewBag.FileMessage);
                }
            }
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(expense), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{baseURL}/api/Expense/CreateExpense", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("ExpenseList");
        }
    }
}
