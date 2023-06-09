using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using HRProject.Entities.Entities;
using HRProject.Entities.Validation;
using HRProject.Service.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HRProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly IGenericService<Company> service;

        public CompanyController(IGenericService<Company> service)
        {
            this.service = service;
        }

        //Listele

        [HttpGet]
        public IActionResult GetAllCompanies()
        {
            var companies = service.GetAll();
            return Ok(companies);
        }

        //Company Özet Bilgileri

        [HttpGet("{id}")]
        public IActionResult GetCompanyById(int id)
        {
            var company = service.GetById(id);
            return Ok(company);
        }

        // Company Add

        [HttpPost]
        public IActionResult CreateCompany([FromBody] Company newCompany)
        {
            newCompany.EmailAddress = newCompany.CreateEmail(newCompany.CompanyName);
            CompanyValidator validator = new CompanyValidator();
            var result = validator.Validate(newCompany);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            else
            {
                var company = service.GetByDefault(x => x.CompanyName == newCompany.CompanyName && x.TaxNumber == newCompany.TaxNumber);
                if (company is not null)
                {
                    List<ValidationFailure> failures = new List<ValidationFailure>
                    {
                        new ValidationFailure
                        {
                            ErrorMessage="The company already exists",
                        },
                    };
                  
                    result.Errors.AddRange(failures);
                   
                    return BadRequest(result.Errors);
                }
                else
                {
                    
                    service.Add(newCompany);
                    return Ok(newCompany);
                }
            }
        }

        //Update

        [HttpPut("{id}")]
        public IActionResult UpdateCompany(int id, [FromBody] Company company)
        {
            company.EmailAddress = company.CreateEmail(company.CompanyName);
            if (company.ContractEndDate <= DateTime.Now)
            {
                company.IsActive = false;
            }
            CompanyValidator validator = new CompanyValidator();
            var result = validator.Validate(company);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            else
            {
                service.Update(company);
                return Ok(company);
            }

        }
    }
}
