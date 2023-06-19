using AutoMapper;
using FluentValidation;
using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using HRProject.Entities.Validation;
using HRProject.UI.Areas.SiteManagement.Models;
using HRProject.UI.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;
using System.Text;

namespace HRProject.UI.Areas.SiteManagement.Controllers
{
    [Area("SiteManagement"), Authorize(Roles = "SiteManager")]
    public class SiteManagementController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment environment;
        string baseURL = "https://localhost:7127/";

        public SiteManagementController(IMapper mapper, IWebHostEnvironment environment)
        {
            _mapper = mapper;
            this.environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            List<SiteManager> siteManagers = new List<SiteManager>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/SiteManager/GetAllSiteManagers"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    siteManagers = JsonConvert.DeserializeObject<List<SiteManager>>(apiCevap);
                }
            }
            var newLoginClaim = new Claim("PhotoUrl", siteManagers[0].PhotoURL);

            var currentUser = HttpContext.User.Identity as ClaimsIdentity;
            var loginClaim = currentUser.FindFirst("PhotoUrl");
            if (loginClaim != null)
            {
                currentUser.RemoveClaim(loginClaim);
                currentUser.AddClaim(newLoginClaim);
            }
            var newPrincipal = new ClaimsPrincipal(currentUser);
            await HttpContext.SignInAsync(newPrincipal);
            return View(siteManagers);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            SiteManager siteManager = new SiteManager();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/SiteManager/GetSiteManagerById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    siteManager = JsonConvert.DeserializeObject<List<SiteManager>>(apiCevap)[0];
                }
            }
            return Json(siteManager);
        }
        static SiteManager updatedSiteManager;

        [HttpGet]
        public async Task<IActionResult> UpdateSiteManager(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/SiteManager/GetSiteManagerById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedSiteManager = JsonConvert.DeserializeObject<List<SiteManager>>(apiCevap)[0];
                }
            }

            UpdateSiteManagerVM vm = _mapper.Map<UpdateSiteManagerVM>(updatedSiteManager);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSiteManager(UpdateSiteManagerVM uservm, List<IFormFile> files)
        {


            if (files.Count == 0) //Foto seçilemez ise
            {
                uservm.PhotoUrl = updatedSiteManager.PhotoURL;
            }
            else
            {

                string returnedMessaage = Upload.ImageUpload(files, environment, out bool imgresult);

                if (imgresult)
                {
                    uservm.PhotoUrl = returnedMessaage;//Eğer ImageUpload'dan fırlatılan değer true ise returnedMessage bana foto url'i döndürcek

                }
                else
                {
                    uservm.PhotoUrl = "/Uploads/ef2fefcf_0dbb_4239_8b28_7f87983acf87.jpeg";
                    ViewBag.PhotoMessage = returnedMessaage;
                    ModelState.AddModelError("", ViewBag.PhotoMessage);

                }
            }


            using (var httpClient = new HttpClient())
            {
                updatedSiteManager.PhotoURL = uservm.PhotoUrl;
                updatedSiteManager.Address = uservm.Address;
                updatedSiteManager.PhoneNumber = uservm.PhoneNumber;
                StringContent content = new StringContent(JsonConvert.SerializeObject(updatedSiteManager), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{baseURL}/api/SiteManager/UpdateSiteManager/{updatedSiteManager.ID}", content))
                {

                    if (!response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var errorResponse = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                        string errorMessage = "";
                        if (errorResponse.errors != null)
                        {
                            var errors = errorResponse.errors;
                            foreach (var error in errors)
                            {
                                var errorMessages = (JArray)error.Value;
                                foreach (var errorMessageToken in errorMessages)
                                {
                                    errorMessage += errorMessageToken.ToString() + "\n";
                                }
                            }
                        }
                        ModelState.AddModelError("", errorMessage); // Hata mesajını ModelState'e ekleyin
                        //var allmasseges = ModelState.ToList();
                        ViewData["allmasseges"] = ModelState.ToList();
                        return View(uservm);
                    }

                    if (!ModelState.IsValid)
                    {
                        ViewData["allmasseges"] = ModelState.ToList();
                        return View(uservm);
                    }


                }
            }

            return RedirectToAction("Index");
        }
    }
}
