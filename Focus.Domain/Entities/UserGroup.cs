using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Domain.Entities
{
    public class UserGroup : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;  
        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;
        public bool Status { get; set; }
        public bool IsAdmin { get; set; }
    }
}
