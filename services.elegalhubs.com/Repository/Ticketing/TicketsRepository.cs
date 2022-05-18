using library.elegalhubs.com.Ticketing;
using Microsoft.EntityFrameworkCore;
using services.elegalhubs.com.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using www.service.com.Data;

namespace services.elegalhubs.com.Repository
{
    public class TicketsRepository : ICommanRepository<Tickets>
    {
        private readonly ApplicationDbContext _context;

        public TicketsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Tickets> Search(Tickets ticketComman)
        {
            throw new NotImplementedException();
        }

        async Task<Tickets> ICommanRepository<Tickets>.Create(Tickets tickets)
        {
            int ticketNo = _context.Tickets.Max(p => (int?)p.TicketNumber) ?? 0;
            tickets.TicketNumber = ticketNo + 1;
            

            _context.Tickets.Add(tickets);
            await _context.SaveChangesAsync();
            return tickets;
        }

        async Task ICommanRepository<Tickets>.DeleteConfirmed(string id)
        {
            using (var context = _context)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var comments = await _context.TicketComments.Where(b => b.TicketId == id).ToListAsync(); 
                        foreach(var m in comments)
                        {
                            _context.TicketComments.Remove(m);
                            await _context.SaveChangesAsync();
                        }

                        var attach = await _context.TicketAttachements.Where(b => b.TicketId == id).ToListAsync(); 
                        foreach(var m in attach)
                        {
                            _context.TicketAttachements.Remove(m);
                            await _context.SaveChangesAsync();
                        }
                        var details = await _context.TicketDetails.Where(b => b.TicketId == id).ToListAsync(); 
                        foreach(var m in details)
                        {
                            _context.TicketDetails.Remove(m);
                            await _context.SaveChangesAsync();
                        }

                        var tickets = await _context.Tickets.FindAsync(id);
                        _context.Tickets.Remove(tickets);
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        async Task<Tickets> ICommanRepository<Tickets>.Details(string id)
        {
            return await _context.Tickets
                .Include(r => r.User)
                .Include(r => r.Department)
                .Include(r => r.Projects)
                .Include(r => r.TicketTypes)
                .Include(r => r.PriorityTypes)
                .Include(r => r.StatusTypes)
                .Include(r => r.CategoryTypes)
                .Include(r => r.ContactMethods)
                .Include(r => r.ContactTypes)
                //.Include(r => r.Employee)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        async Task ICommanRepository<Tickets>.Edit(Tickets tickets)
        {
            _context.Entry(tickets).State = EntityState.Modified;

            Type type = typeof(Tickets);
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.GetValue(tickets, null) == null)
                {
                    _context.Entry(tickets).Property(property.Name).IsModified = false;
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task Edit(EditTicketVM tickets)
        {
            var entity = _context.Tickets.FirstOrDefault(item => item.Id == tickets.Id);
            if (entity != null)
            {
                //entity.AssignTo = (tickets.AssignTo is null ? entity.AssignTo : tickets.AssignTo);
                entity.PriorityId = (tickets.PriorityId is null ? entity.PriorityId : tickets.PriorityId);
                entity.StatusId = (tickets.StatusId is null ? entity.StatusId: tickets.StatusId);
                entity.DepartmentId = (tickets.DepartmentId is null ? entity.DepartmentId : tickets.DepartmentId);
                await _context.SaveChangesAsync();
            }
        }

        async Task<IEnumerable<Tickets>> ICommanRepository<Tickets>.Index()
        {
            return await _context.Tickets
                .Include(r => r.User)
                .Include(r => r.Department)
                .Include(r => r.Projects)
                .Include(r => r.TicketTypes)
                .Include(r => r.PriorityTypes)
                .Include(r => r.StatusTypes)
                .Include(r => r.CategoryTypes)
                .Include(r => r.ContactTypes)
                .OrderByDescending(m => m.TicketNumber)
                .ToListAsync();
        }

        Task<IEnumerable<Tickets>> ICommanRepository<Tickets>.Search(Tickets ticketComman)
        {
            throw new NotImplementedException();
        }
    }
}
