using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProject.Entities.Entities
{
    public class Job : BaseEntity
    {
        public ICollection<User> Users { get; set; }
    }
}
