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
    public class AdvancePaymentController : Controller
    {
        string baseURL = "https://localhost:7127";
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment environment;

        public AdvancePaymentController(IMapper mapper, IWebHostEnvironment environment)
        {
            _mapper = mapper;
            this.environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAdvancePaymentRequest()
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

            List<AdvancePayment> permissions = new List<AdvancePayment>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/AdvancePayment/GetAdvancePaymentRequestbyCompanyId/{companyManagers.CompanyId}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    permissions = JsonConvert.DeserializeObject<List<AdvancePayment>>(apiCevap);
                }
            }
            return View(permissions);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmedAdvancePayment(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/AdvancePayment/ConfirmedAdvancePayment/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("GetAdvancePaymentRequest");
        }
        [HttpGet]
        public async Task<IActionResult> CancelledAdvancePayment(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/AdvancePayment/CancelledAdvancePayment/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("GetAdvancePaymentRequest");
        }
    }
}