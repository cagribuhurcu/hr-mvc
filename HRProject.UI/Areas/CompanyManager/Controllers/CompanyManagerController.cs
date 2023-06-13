using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using HRProject.UI.Areas.SiteManagement.Models;
using HRProject.UI.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace HRProject.UI.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager"), Authorize(Roles = "CompanyManager")]
    public class CompanyManagerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment environment;
        string baseURL = "https://localhost:7127";

        public CompanyManagerController(IMapper mapper, IWebHostEnvironment environment)
        {
            _mapper = mapper;
            this.environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            List<CompanyManagerEntity> companyManagers = new List<CompanyManagerEntity>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/CompanyManager/GetAllCompanyManagers"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    companyManagers = JsonConvert.DeserializeObject<List<CompanyManagerEntity>>(apiCevap);
                }
            }
            var newLoginClaim = new Claim("PhotoUrl", companyManagers[0].PhotoURL);

            var currentCompanyManager = HttpContext.User.Identity as ClaimsIdentity;
            var loginClaim = currentCompanyManager.FindFirst("PhotoUrl");
            if (loginClaim != null)
            {
                currentCompanyManager.RemoveClaim(loginClaim);
                currentCompanyManager.AddClaim(newLoginClaim);
            }
            var newPrincipal = new ClaimsPrincipal(currentCompanyManager);
            await HttpContext.SignInAsync(newPrincipal);
            return View(companyManagers);
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
        //static User updateduser;

        //[HttpGet]
        //public async Task<IActionResult> UpdateUser(int id)
        //{

        //    using (var httpClient = new HttpClient())
        //    {
        //        using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/GetUserById/{id}"))
        //        {
        //            string apiCevap = await cevap.Content.ReadAsStringAsync();
        //            updateduser = JsonConvert.DeserializeObject<List<User>>(apiCevap)[0];
        //        }
        //    }

        //    UpdateUserVM vm = _mapper.Map<UpdateUserVM>(updateduser);

        //    return View(vm);
        //}

        //[HttpPost]
        //public async Task<IActionResult> UpdateUser(UpdateUserVM uservm, List<IFormFile> files)
        //{


        //    if (files.Count == 0) //Foto seçilemez ise
        //    {
        //        uservm.PhotoUrl = updateduser.PhotoURL;
        //    }
        //    else
        //    {

        //        string returnedMessaage = Upload.ImageUpload(files, environment, out bool imgresult);

        //        if (imgresult)
        //        {
        //            uservm.PhotoUrl = returnedMessaage;//Eğer ImageUpload'dan fırlatılan değer true ise returnedMessage bana foto url'i döndürcek

        //        }
        //        else
        //        {
        //            uservm.PhotoUrl = "/Uploads/ef2fefcf_0dbb_4239_8b28_7f87983acf87.jpeg";
        //            ViewBag.PhotoMessage = returnedMessaage;
        //            ModelState.AddModelError("", ViewBag.PhotoMessage);

        //        }
        //    }


        //    using (var httpClient = new HttpClient())
        //    {
        //        updateduser.PhotoURL = uservm.PhotoUrl;
        //        updateduser.Address = uservm.Address;
        //        updateduser.PhoneNumber = uservm.PhoneNumber;
        //        StringContent content = new StringContent(JsonConvert.SerializeObject(updateduser), Encoding.UTF8, "application/json");

        //        using (var response = await httpClient.PutAsync($"{baseURL}/api/User/UpdateUser/{updateduser.ID}", content))
        //        {

        //            if (!response.IsSuccessStatusCode)
        //            {
        //                var jsonResponse = await response.Content.ReadAsStringAsync();
        //                var errorResponse = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

        //                string errorMessage = "";
        //                if (errorResponse.errors != null)
        //                {
        //                    var errors = errorResponse.errors;
        //                    foreach (var error in errors)
        //                    {
        //                        var errorMessages = (JArray)error.Value;
        //                        foreach (var errorMessageToken in errorMessages)
        //                        {
        //                            errorMessage += errorMessageToken.ToString() + "\n";
        //                        }
        //                    }
        //                }
        //                ModelState.AddModelError("", errorMessage); // Hata mesajını ModelState'e ekleyin
        //                //var allmasseges = ModelState.ToList();
        //                ViewData["allmasseges"] = ModelState.ToList();
        //                return View(uservm);
        //            }

        //            if (!ModelState.IsValid)
        //            {
        //                ViewData["allmasseges"] = ModelState.ToList();
        //                return View(uservm);
        //            }


        //        }
        //    }

        //    return RedirectToAction("Index");
        //}
    }
}
