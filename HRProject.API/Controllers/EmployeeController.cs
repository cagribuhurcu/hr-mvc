using HRProject.Entities.Entities;
using HRProject.Entities.Validation;
using HRProject.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IGenericRepository<Employee> service;
        public EmployeeController(IGenericRepository<Employee> service)
        {
            this.service = service;
        }

        //Employee yaratmak için
        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            if (employee.FirstName != null && employee.LastName != null)
            {
                employee.EmailAddress = employee.CreateEmail(employee.FirstName, employee.MiddleName, employee.LastName);
            }
            EmployeeValidator validator = new EmployeeValidator();
            var result = validator.Validate(employee);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            else
            {
                var emp = service.GetByDefault(x => x.IdentificationNumber == employee.IdentificationNumber);
                if (emp is not null)
                {
                    return BadRequest("The employee already exist");
                }
                else
                {
                    service.Add(employee);
                    return Ok(employee);
                }
            }
        }
    }
}
