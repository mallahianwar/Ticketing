using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace services.elegalhubs.com.Model
{
    public class TicketVM
    {
        public string TicketId { get; set; }
    }
    public class EditTicketVM
    {
        public string Id { get; set; }
        public string StatusId { get; set; }
        public string PriorityId { get; set; } 
        public string PreferredContactMethodId { get; set; }
        public string AssignTo { get; set; }

        public string DepartmentId { get; set; }

        public bool IsDelete { get; set; } = false;
    }
}
