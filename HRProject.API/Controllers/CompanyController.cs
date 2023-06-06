using HRProject.Entities.Entities;
using HRProject.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace HRProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly IGenericService<Company> service;

        public CompanyController(IGenericService<Company> service)
        {
            this.service = service;
        }

        //Listele

        [HttpGet]
        public IActionResult GetAllCompanies()
        {
            var companies = service.GetAll();
            return Ok(companies);
        }

        //Company Özet Bilgileri

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var company = service.GetById(id);
            return Ok(company);
        }
    }
}
