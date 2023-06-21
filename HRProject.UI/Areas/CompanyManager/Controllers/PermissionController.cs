﻿using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;

namespace HRProject.UI.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager"), Authorize(Roles = "CompanyManager")]
    public class PermissionController : Controller
    {
        string baseURL = "https://localhost:7127";
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment environment;

        public PermissionController(IMapper mapper, IWebHostEnvironment environment)
        {
            _mapper = mapper;
            this.environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissionRequest()
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

            List<EmployeePermission> permissions=new List<EmployeePermission>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Permission/GetPermissionRequestbyCompanyId/{companyManagers.CompanyId}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    permissions = JsonConvert.DeserializeObject<List<EmployeePermission>>(apiCevap);
                }
            }
            return View(permissions);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmedPermission(int id)
        {
            using(var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Permission/ConfirmedPermission/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("GetPermissionRequest");
        }
        [HttpGet]
        public async Task<IActionResult> CancelledPermission(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/Permission/CancelledPermission/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("GetPermissionRequest");
        }
    }
}
