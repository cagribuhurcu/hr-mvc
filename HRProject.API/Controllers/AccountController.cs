﻿using AutoMapper;
using HRProject.Entities.Entities;
using HRProject.Entities.Enums;
using HRProject.Repositories.Abstract;
using HRProject.UI.Areas.SiteManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IGenericRepository<SiteManager> service;
        private readonly IGenericRepository<CompanyManagerEntity> companyManagerService;
        private readonly IGenericRepository<Employee> employeeService;
        private readonly IGenericRepository<User> userService;

        public AccountController(IGenericRepository<SiteManager> service,IGenericRepository<CompanyManagerEntity> companyManagerService, IGenericRepository<Employee> employeeService,IGenericRepository<User> userService)
        {
            this.service = service;
            this.companyManagerService = companyManagerService;
            this.employeeService = employeeService;
            this.userService = userService;
        }

        [HttpGet("{identificationNumber}")]
        public IActionResult GetUserByIdentificationNumber(string identificationNumber)
        {
            if (service.Any(x=> x.IdentificationNumber==identificationNumber))
            {
                SiteManager user = service.GetByDefault(x => x.IdentificationNumber == identificationNumber);
                return Ok(user);
            }
            else if (companyManagerService.Any(x => x.IdentificationNumber == identificationNumber))
            {
                CompanyManagerEntity user = companyManagerService.GetByDefault(x => x.IdentificationNumber == identificationNumber);
                return Ok(user);
            }
            else if (employeeService.Any(x => x.IdentificationNumber == identificationNumber))
            {
                Employee user = employeeService.GetByDefault(x => x.IdentificationNumber == identificationNumber);
                return Ok(user);
            }
            return BadRequest("asdasd");
        }

        [HttpGet]
        public IActionResult Login(string email, string password)
        {
            if (service.Any(x => x.EmailAddress == email))
            {
                SiteManager loggeduser = service.GetByDefault(x => x.EmailAddress == email && x.Password == password);
                if (loggeduser != null)
                    return Ok(loggeduser);

                else
                    return BadRequest("Email Address or Password is incorrect!");
            }
            else if (companyManagerService.Any(x => x.EmailAddress == email))
            {
                CompanyManagerEntity loggeduser = companyManagerService.GetByDefault(x => x.EmailAddress == email && x.Password == password);
                if (loggeduser != null)
                    return Ok(loggeduser);

                else
                    return BadRequest("Email Address or Password is incorrect!");
            }
            else if (employeeService.Any(x => x.EmailAddress == email))
            {
                Employee loggeduser = employeeService.GetByDefault(x => x.EmailAddress == email && x.Password == password); // degiscek
                if (loggeduser != null)
                    return Ok(loggeduser);

                else
                    return BadRequest("Email Address or Password is incorrect!");
            }
            return NotFound("User not found");
        }

        [HttpPut("{identificationNum}")]
        public IActionResult FirstPasswordChange(string identificationNum, [FromBody] User user)
        {
            if (service.Any(x => x.IdentificationNumber == identificationNum))
            {
                SiteManager cUser = service.GetByDefault(a => a.IdentificationNumber == identificationNum);
                cUser.Password = user.Password;
                cUser.IsPasswordChange = true;
                if (cUser != null)
                {
                    service.Update(cUser);
                    return Ok(cUser);
                }
                
            }
            else if (companyManagerService.Any(x => x.IdentificationNumber == identificationNum))
            {
                CompanyManagerEntity cUser = companyManagerService.GetByDefault(a => a.IdentificationNumber == identificationNum);
                cUser.Password = user.Password;
                cUser.IsPasswordChange = true;
                if (cUser != null)
                {
                    companyManagerService.Update(cUser);
                    return Ok(cUser);
                }
               
            }
            else if (employeeService.Any(x => x.IdentificationNumber == identificationNum))
            {
                Employee cUser = employeeService.GetByDefault(a => a.IdentificationNumber == identificationNum);
                cUser.Password = user.Password;
                cUser.IsPasswordChange = true;
                if (cUser != null)
                {
                    employeeService.Update(cUser);
                    return Ok(cUser);
                }
                
            }
            return NotFound("User not found");
        }
    }
}
