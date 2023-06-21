using HRProject.Entities.Entities;
using HRProject.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdvancePaymentController : ControllerBase
    {
        public readonly IGenericRepository<AdvancePayment> service;
        public AdvancePaymentController(IGenericRepository<AdvancePayment> service)
        {
            this.service = service;
        }

        [HttpPost]
        public IActionResult CreateAdvancePayment([FromBody] AdvancePayment advancePayment)
        {
            service.Add(advancePayment);
            return Ok(advancePayment);
        }

        [HttpGet("{id}")]
        public IActionResult GetAllAdvancePaymentbyEmployeeId(int id)
        {
            var advancePayments = service.GetAll().Where(x=> x.EmployeeID==id).ToList();
            return Ok(advancePayments);
        }
        [HttpGet]
        public IActionResult GetAllAdvancePayment()
        {
            return Ok(service.GetAll());
        }
    }
}
