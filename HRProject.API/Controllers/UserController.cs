using HRProject.Entities.Entities;
using HRProject.Entities.Validation;
using HRProject.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace HRProject.API.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository<User> service;

        public UserController(IGenericRepository<User> service)
        {
            this.service= service;
        }

        //Listele

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = service.GetAll(t0=> t0.Job);
            return Ok(users);
        }

        //Site Yöneticisi Özet Bilgileri

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = service.GetById(id,t0=>t0.Job);
            return Ok(user);
        }

        //Güncelleme

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (user.QuitDate!=null)
            {
                user.IsActive = false;
            }
            UserValidator validator = new UserValidator();
            var result = validator.Validate(user);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            else
            {
                service.Update(user);
                return Ok(user);
            }

        }
        //Create

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            UserValidator validator = new UserValidator();
            var result = validator.Validate(user);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            else
            {
                service.Add(user);
                return Ok(user);
            }
        }
    }
}
