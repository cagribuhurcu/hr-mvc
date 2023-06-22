using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace HRProject.UI.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager"), Authorize(Roles = "CompanyManager")]
    public class ExpenseController : Controller
    {
        string baseURL = "https://localhost:7127";
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment environment;

        public ExpenseController(IMapper mapper, IWebHostEnvironment environment)
        {
            _mapper = mapper;
            this.environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenseRequest()
        {
            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginIdClaim = currentUser.FindFirst("ID");

            CompanyManagerEntity companyManagers = new CompanyManagerEntity();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/CompanyManager/GetCompanyManagerById/{loginIdClaim.Value}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    companyManagers = JsonConvert.DeserializeObject<List<CompanyManagerEntity>>(apiCevap)[0];
                }
            }

            List<Expense> expenses = new List<Expense>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Expense/GetExpenseRequestbyCompanyId/{companyManagers.CompanyId}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    expenses = JsonConvert.DeserializeObject<List<Expense>>(apiCevap);
                }
            }
            return View(expenses);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmedExpense(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Expense/ConfirmedExpense/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("GetExpenseRequest");
        }
        [HttpGet]
        public async Task<IActionResult> CancelledExpense(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Expense/CancelledExpense/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("GetExpenseRequest");
        }
    }
}
