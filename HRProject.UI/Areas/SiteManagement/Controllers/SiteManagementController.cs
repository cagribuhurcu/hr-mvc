using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using HRProject.UI.Areas.SiteManagement.Models;
using HRProject.UI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text;

namespace HRProject.UI.Areas.SiteManagement.Controllers
{
    [Area("SiteManagement") /*Authorize(Roles = "Admin")*/]
    public class SiteManagementController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment environment;
        string baseURL = "https://localhost:7270";

        public SiteManagementController(IMapper mapper, IWebHostEnvironment environment)
        {
            _mapper = mapper;
            this.environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            List<User> users = new List<User>();

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/GetAllUsers"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<List<User>>(apiCevap);
                }
            }

            List<UserVM> userVMs = _mapper.Map<List<UserVM>>(users);

            return View(userVMs);
        }

          
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            User user=new User();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/GetUserById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(apiCevap);
                }
            }
            return View(user);
        }
        static User updateduser;
       
        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {
           
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{baseURL}/api/User/GetUserById/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updateduser = JsonConvert.DeserializeObject<User>(apiCevap);
                }
            }

            UpdateUserVM vm =_mapper.Map<UpdateUserVM>(updateduser);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserVM uservm, List<IFormFile> files)
        {
            if (files.Count == 0) //Foto seçilemz ise
            {
                uservm.PhotoUrl = updateduser.PhotoURL;
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
                    ViewBag.PhotoMessage = returnedMessaage;
                    //updateduser = _mapper.Map<UpdateUserVM>(uservm);
                    return View(uservm);
                }
            }
            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(updateduser), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{baseURL}/api/User/UpdateUser/{updateduser.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Index");
        }

    }
}
