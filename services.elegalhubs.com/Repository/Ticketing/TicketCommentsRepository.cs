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
    public class TicketCommentsRepository : ICommanRepository<TicketComments>
    {
        private readonly ApplicationDbContext _context;

        public TicketCommentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        

        async Task<TicketComments> ICommanRepository<TicketComments>.Create(TicketComments TicketComments)
        {
            _context.TicketComments.Add(TicketComments);
            await _context.SaveChangesAsync();
            return TicketComments;
        }

        async Task ICommanRepository<TicketComments>.DeleteConfirmed(string id)
        {
            var TicketComments = await _context.TicketComments.FindAsync(id);
            _context.TicketComments.Remove(TicketComments);
            await _context.SaveChangesAsync();
        }

        async Task<TicketComments> ICommanRepository<TicketComments>.Details(string id)
        {
            return await _context.TicketComments.Include(r => r.user).FirstOrDefaultAsync(i => i.Id == id);
        }

        async Task ICommanRepository<TicketComments>.Edit(TicketComments TicketComments)
        {
            _context.Entry(TicketComments).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

       async Task<IEnumerable<TicketComments>> ICommanRepository<TicketComments>.Index()
        {
            return await _context.TicketComments.ToListAsync();
        }

        async Task<IEnumerable<TicketComments>> ICommanRepository<TicketComments>.Search(TicketComments ticketComman)
        {
            return await _context.TicketComments
                    .Where(b => b.TicketId == ticketComman.TicketId).ToListAsync();
        }
        public async Task<IEnumerable<TicketComments>> SearchVM(TicketVM ticketComman)
        {
            return await _context.TicketComments
                    .Where(b => b.TicketId == ticketComman.TicketId).Include(m => m.user).ToListAsync();
        }
    }
}
