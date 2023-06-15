using HRProject.Entities.Entities;
using HRProject.Entities.Validation;
using HRProject.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace HRProject.API.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SiteManagerController : ControllerBase
    {
        private readonly IGenericRepository<SiteManager> service;
        private readonly IGenericRepository<CompanyManagerEntity> companyManagerService;
        private readonly IGenericRepository<Employee> employeeService;

        public SiteManagerController(IGenericRepository<SiteManager> service,IGenericRepository<CompanyManagerEntity> companyManagerService,IGenericRepository<Employee> employeeService)
        {
            this.service = service;
            this.companyManagerService = companyManagerService;
            this.employeeService = employeeService;
        }

        //Listele

        [HttpGet]
        public IActionResult GetAllSiteManagers()
        {
            var siteManagers = service.GetAll(t0 => t0.Job);
            return Ok(siteManagers);
        }

        //Site Yöneticisi Özet Bilgileri

        [HttpGet("{id}")]
        public IActionResult GetSiteManagerById(int id)
        {
            var siteManager = service.GetById(id, t0 => t0.Job);
            return Ok(siteManager);
        }

        //Güncelleme

        [HttpPut("{id}")]
        public IActionResult UpdateSiteManager(int id, [FromBody] SiteManager siteManager)
        {
            if (siteManager.QuitDate != null)
            {
                siteManager.IsActive = false;
            }
            UserValidator validator = new UserValidator();
            var result = validator.Validate(siteManager);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            else
            {
                service.Update(siteManager);
                return Ok(siteManager);
            }

        }
        //Create

        [HttpPost]
        public IActionResult CreateSiteManager([FromBody] SiteManager siteManager)
        {
            siteManager.EmailAddress = siteManager.CreateEmail(siteManager.FirstName, siteManager.LastName);
            UserValidator validator = new UserValidator();
            var result = validator.Validate(siteManager);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            else
            {
                service.Add(siteManager);
                return Ok(siteManager);
            }
        }

        //[HttpGet]
        //public IActionResult Login(string email, string password)
        //{
        //    if (service.Any(x => x.EmailAddress == email))
        //    {
        //        SiteManager loggeduser = service.GetByDefault(x => x.EmailAddress == email && x.Password == password);
        //        if (loggeduser != null)
        //            return Ok(loggeduser);

        //        else
        //            return BadRequest("Email Address or Password is incorrect!");
        //    }
        //    else if (companyManagerService.Any(x => x.EmailAddress == email))
        //    {
        //        CompanyManagerEntity loggeduser = companyManagerService.GetByDefault(x => x.EmailAddress == email && x.Password == password);
        //        if (loggeduser != null)
        //            return Ok(loggeduser);

        //        else
        //            return BadRequest("Email Address or Password is incorrect!");
        //    }
        //    else if (employeeService.Any(x => x.EmailAddress == email))
        //    {
        //        Employee loggeduser = employeeService.GetByDefault(x => x.EmailAddress == email && x.Password == password); // degiscek
        //        if (loggeduser != null)
        //            return Ok(loggeduser);

        //        else
        //            return BadRequest("Email Address or Password is incorrect!");
        //    }
        //    return NotFound("User not found");
        //}
    }
}
