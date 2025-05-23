using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Domain.Entities
{
    public class UserGroup : BaseEntity
    {
        public User User { get; set; }

        public Group Group { get; set; }

        public DateTime DateEntered { get; set; }

        public bool Status { get; set; }

        public bool IsOwner { get; set; }
    }
}
