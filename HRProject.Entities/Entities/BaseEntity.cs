using HRProject.Entities.Enums;

namespace HRProject.Entities.Entities
{
    public class BaseEntity
    {
        public int ID { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
