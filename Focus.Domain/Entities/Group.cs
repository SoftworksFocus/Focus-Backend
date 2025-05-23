using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Domain.Entities
{
    public class Group : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? MyProperty { get; set; }

        public User Admin { get; set; }
    }
}
