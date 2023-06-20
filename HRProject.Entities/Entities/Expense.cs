using HRProject.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Entities
{
    public class Expense : BaseEntity
    {
        public ExpenseType? ExpenseType { get; set; }
        public decimal? Amount { get; set; }
        public Currency? Currency { get; set; }
        public PermissionState? ApprovalStatus { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? FileURL { get; set; }
        public int? EmployeeID { get; set; }
        public Employee? Employee { get; set; }
    }
}
