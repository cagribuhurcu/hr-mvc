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

        public UserController(IGenericRepository<User> service)
        {
            this.service= service;
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
        public IActionResult CreateUser([FromQuery] User user)
        {
            service.Add(user);
            return Ok(user);
        }
    }
}
