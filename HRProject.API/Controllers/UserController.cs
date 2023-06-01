using HRProject.Entities.Entities;
using HRProject.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace HRProject.API.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository<User> service;

        public IGenericRepository<Job> JobService { get; }

        public UserController(IGenericRepository<User> service, IGenericRepository<Job> jobService)
        {
            this.service= service;
            JobService = jobService;
        }

        //Listele

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = service.GetAll();
            return Ok(users);
        }

        //Site Yöneticisi Özet Bilgileri

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            return Ok(service.GetById(id));
        }

        //Güncelleme

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            service.Update(user);
            return Ok(user);

        }
        //Create

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            //var job = JobService.GetByDefault(x => x.ID == user.JobID);
            //user.Job = job;
            service.Add(user);
            return Ok(user);
        }
    }
}
