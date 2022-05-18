using library.elegalhubs.com.lms.Admin.Chat;
using library.elegalhubs.com.Notification;
using library.elegalhubs.com.Ticketing;
using System;
using System.Collections.Generic;
using System.Text;

namespace library.elegalhubs.com.lms.Admin.Reports
{
    public class IndexReport
    {
        public int ProjectsCount { get; set; }
        public int ContactsCount { get; set; }
        public int TicketsCount { get; set; }
        public int CompleteTicketsCount { get; set; }
        public int PendingTicketsCount { get; set; }
        public int OpenTicketsCount { get; set; }

        public IEnumerable<Tickets> Tickets { get; set; }
        public IEnumerable<ContactTypes> Contacts { get; set; }
        public IEnumerable<TicketComments> TicketComments { get; set; }
        public IEnumerable<UsersMessages> UsersMessages { get; set; }

    }
}
