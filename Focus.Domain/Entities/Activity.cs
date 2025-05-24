using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Domain.Entities
{
    public class Activity : BaseEntity
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }

        public bool Status { get; set; } = true;

        public int UserId { get; set; }
        public User User { get; set; }  = null!;

        public int? GroupId { get; set; }
        public Group? Group { get; set; }
    }
}
