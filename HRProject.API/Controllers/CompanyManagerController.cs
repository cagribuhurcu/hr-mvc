using HRProject.Entities.Entities;
using HRProject.Entities.Validation;
using HRProject.Repositories.Abstract;
using HRProject.UI.Helper;
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

        //Create

        [HttpPost]
        public IActionResult CreateCompanyManager([FromBody] CompanyManagerEntity companyManager)
        {
            if (companyManager.FirstName != null && companyManager.LastName != null)
            {
                companyManager.EmailAddress = companyManager.CreateEmail(companyManager.FirstName, companyManager.MiddleName, companyManager.LastName);
            }
            CompanyManagerValidator validator = new CompanyManagerValidator();
            var result = validator.Validate(companyManager);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            else
            {
                var company = service.GetByDefault(x => x.IdentificationNumber == companyManager.IdentificationNumber);
                if (company is not null)
                {
                    return BadRequest("The company manager already exist");
                }
                else
                {
                    service.Add(companyManager);
                    return Ok(companyManager);
                }
            }
        }

        //Update

        [HttpPut("{id}")]
        public IActionResult UpdateCompanyManager(int id, [FromBody] CompanyManagerEntity companyManager)
        {
            if (companyManager.QuitDate <= DateTime.Now)
            {
                companyManager.IsActive = false;
            }
            CompanyManagerValidator validator = new CompanyManagerValidator();
            var result = validator.Validate(companyManager);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            else
            {
                service.Update(companyManager);
                return Ok(companyManager);
            }

        }
    }
}
