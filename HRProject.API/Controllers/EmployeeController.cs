﻿using HRProject.Entities.Entities;
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
        private readonly IGenericRepository<EmployeePermission> emppermissionservice;

        public EmployeeController(IGenericRepository<Employee> service,IGenericRepository<EmployeePermission> emppermissionservice)
        {
            this.service = service;
            this.emppermissionservice = emppermissionservice;
        }
        //Listelemek için kullanılan action 

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = service.GetAll(t0 => t0.Company, t1 => t1.Job);
            return Ok(employees);
        }

        //Personel Özet Bilgileri

        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = service.GetById(id, t0 => t0.Company, t1 => t1.Job);
            return Ok(employee);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAnnualDay(int id,[FromBody] Employee emp)
        {
            service.Update(emp);
            return Ok(emp);
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
