using HRProject.Entities.Entities;
using HRProject.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{id}")]
        public IActionResult GetAllPermissionbyEmployeeId(int id)
        {
            var permissions = empPermissionService.GetAll(a=>a.Permission,b=>b.Employee).Where(a => a.EmployeeId == id).ToList();
            return Ok(permissions);
        }
    }
}
