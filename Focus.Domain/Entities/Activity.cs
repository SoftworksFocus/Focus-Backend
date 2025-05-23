using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Domain.Entities
{
    public class Activity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }

        public DateTime CreationDate { get; set; }

        public User User { get; set; }

        public Group? Group { get; set; }
    }
}
