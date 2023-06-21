using HRProject.Entities.Entities;
using HRProject.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Permissions;

namespace HRProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IGenericRepository<Permission> permissionService;
        private readonly IGenericRepository<EmployeePermission> empPermissionService;

        public PermissionController(IGenericRepository<Permission> permissionService, IGenericRepository<EmployeePermission> empPermissionService)
        {
            this.permissionService = permissionService;
            this.empPermissionService = empPermissionService;
        }

        [HttpPost]
        public IActionResult CreatePermission([FromBody] Permission permission)
        {
            permissionService.Add(permission);
            return Ok(permission);
        }

        [HttpPost]
        public IActionResult CreatePermissionforEmployee([FromBody] EmployeePermission employeePermission)
        {
            empPermissionService.Add(employeePermission);
            return Ok(employeePermission);
        }
        [HttpDelete("{id}")]
        public IActionResult DeletePermission(int id)
        {
            var employeePermission = empPermissionService.GetById(id);
            empPermissionService.Remove(employeePermission);
            return Ok(employeePermission);
        }

        [HttpGet("{id}")]
        public IActionResult GetAllPermissionbyEmployeeId(int id)
        {
            var permissions = empPermissionService.GetAll(a=>a.Permission,b=>b.Employee).Where(a => a.EmployeeId == id).ToList();
            return Ok(permissions);
        }

        [HttpGet]
        public IActionResult GetAllPermission()
        {
            return Ok(permissionService.GetAll());
        }
        [HttpGet("{id}")]
        public IActionResult GetEmployeePermissionbyId(int id)
        {
            return Ok(empPermissionService.GetById(id));
        }
        [HttpGet("{id}")]
        public IActionResult GetPermissionRequestbyCompanyId(int id)
        {
           
            var permissions = empPermissionService.GetAll(a => a.Permission, b => b.Employee).Where(a => a.Employee.CompanyId==id).ToList();
            return Ok(permissions);
        }
        [HttpGet("{id}")]
        public IActionResult ConfirmedPermission(int id)
        {
            var employeeperm=empPermissionService.GetById(id);
            employeeperm.ConfirmedDate = DateTime.Now;
            employeeperm.PermissionState = Entities.Enums.Status.Confirmed;
            empPermissionService.Update(employeeperm);
            return Ok(employeeperm);
        }
        [HttpGet("{id}")]
        public IActionResult CancelledPermission(int id)
        {
            var employeeperm = empPermissionService.GetById(id);
            employeeperm.ConfirmedDate = DateTime.Now;
            employeeperm.PermissionState = Entities.Enums.Status.Canceled;
            empPermissionService.Update(employeeperm);
            return Ok(employeeperm);
        }
    }
}
