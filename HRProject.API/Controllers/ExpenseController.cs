using HRProject.Entities.Entities;
using HRProject.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        public readonly IGenericRepository<Expense> service;
        public ExpenseController(IGenericRepository<Expense> service)
        {
            this.service = service;
        }

        [HttpPost]
        public IActionResult CreateExpense([FromBody] Expense expense)
        {
            service.Add(expense);
            return Ok(expense);
        }

        [HttpGet("{id}")]
        public IActionResult GetAllExpensebyEmployeeId(int id)
        {
            var expenses = service.GetAll().Where(x => x.EmployeeID == id).ToList();
            return Ok(expenses);
        }
        [HttpGet]
        public IActionResult GetAllExpenses()
        {
            return Ok(service.GetAll());
        }
    }
}
