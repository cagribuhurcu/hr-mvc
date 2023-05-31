using HRProject.Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace HRProject.Entities.Entities
{
    public class BaseEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Departments Department { get; set; }
    }
}
