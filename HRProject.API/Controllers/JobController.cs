using HRProject.Entities.Entities;
using HRProject.Service.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IGenericService<Job> service;
        public JobController(IGenericService<Job> service)
        {
            this.service = service;
        }

        [HttpPost]
        public IActionResult CreateJob([FromBody] Job job)
        {
            service.Add(job);
            return Ok(job);
        }
    }
}
