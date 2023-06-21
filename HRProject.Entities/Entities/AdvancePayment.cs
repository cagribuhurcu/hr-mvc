using HRProject.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Entities
{
    public class AdvancePayment : BaseEntity
    {
        public AdvancePaymentType? AdvancePaymentType { get; set; }
        public DateTime? RequestDate { get; set; }
        public Status? ApprovalStatus { get; set; } = Status.Pending;
        public DateTime? ReplyDate { get; set; }
        public Currency? Currency { get; set; }
        public string? Description { get; set; }
        public int? EmployeeID { get; set; }
        public Employee? Employee { get; set; }
        public decimal? Amount { get; set; }
    }
}
