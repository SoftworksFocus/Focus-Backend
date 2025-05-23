using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Focus.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public List<Group> OwnedGroups { get; set; }

        public string? Description { get; set; }
    }
}
