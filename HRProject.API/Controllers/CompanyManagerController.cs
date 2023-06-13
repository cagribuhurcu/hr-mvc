using HRProject.Entities.Entities;
using HRProject.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CompanyManagerController : ControllerBase
    {
        private readonly IGenericRepository<CompanyManagerEntity> service;
        public CompanyManagerController(IGenericRepository<CompanyManagerEntity> service)
        {
            this.service = service;
        }

        //Listelemek için kullanılan action 

        [HttpGet]
        public IActionResult GetAllCompanyManagers()
        {
            var companyManagers = service.GetAll(t0 => t0.Company, t1 => t1.Job);
            return Ok(companyManagers);
        }

        //Şirket Yöneticisi Özet Bilgileri

        [HttpGet("{id}")]
        public IActionResult GetCompanyManagerById(int id)
        {
            var companyManager = service.GetById(id,t0 => t0.Company, t1 => t1.Job);
            return Ok(companyManager);
        }
    }
}
