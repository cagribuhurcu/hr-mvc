//using HRProject.Repositories.Abstract;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace HRProject.API.Controllers
//{
//    [Route("api/[controller]/[action]")]
//    [ApiController]
//    public class CompanyManagerController : ControllerBase
//    {
//        private readonly IGenericRepository<CompanyManager> service;
//        public CompanyManagerController(IGenericRepository<CompanyManager> service)
//        {
//            this.service = service;
//        }

//        //Listelemek için kullanılan action 

//        [HttpGet]
//        public IActionResult GetAllCompanyManagers()
//        {
//            var companyManagers = service.GetAll();
//            return Ok (companyManagers);
//        }

//        //Şirket Yöneticisi Özet Bilgileri

//        [HttpGet("{id}")]
//        public IActionResult GetCompanyManagerById(int id)
//        {
//            var companyManager = service.GetById(id);
//            return Ok(companyManager);
//        }
//    }
//}
