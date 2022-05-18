using library.elegalhubs.com.lms.Admin.Reports;
using library.elegalhubs.com.Ticketing;
using Microsoft.EntityFrameworkCore;
using services.elegalhubs.com.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using www.service.com.Data;

namespace services.elegalhubs.com.Repository
{
    public class ReportsRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IndexReport> IndexReport()
        {
            IndexReport rprt = new IndexReport();
            rprt.ProjectsCount = _context.Projects.Count();
            rprt.TicketsCount = _context.Tickets.Count();
            rprt.ContactsCount = _context.ContactTypes.Count();
            rprt.OpenTicketsCount = _context.Tickets.Where(e => e.StatusId == "1").Count(); // 1 open
            rprt.PendingTicketsCount = _context.Tickets.Where(e => e.StatusId == "2").Count();//3 pending or in progress
            rprt.CompleteTicketsCount = _context.Tickets.Where(e => e.StatusId == "3").Count();// 2 closed or complete
            rprt.Tickets = await _context.Tickets.OrderByDescending(r => r.OpenDateTime)
                .Include(r => r.Projects).Include(r => r.PriorityTypes).Take(5).ToListAsync();
            rprt.TicketComments = await _context.TicketComments.OrderByDescending(r => r.CommentDate)
                .Include(r => r.Tickets)
                .Include(r => r.user)
                .Take(5).ToListAsync();
            rprt.Contacts = await _context.ContactTypes.OrderByDescending(r => r.CreatedAt)
                .Take(5).ToListAsync();
            return rprt;
        }

    }
}
