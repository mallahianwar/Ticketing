using library.elegalhubs.com.Ticketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lms.elegalhubs.com.Areas.Admin.Models
{
    public class ProjectsTicketsViewModel
    {
        public Projects projects { get; set; }
        public library.elegalhubs.com.Ticketing.Tickets tickets { get; set; }
    }
}
