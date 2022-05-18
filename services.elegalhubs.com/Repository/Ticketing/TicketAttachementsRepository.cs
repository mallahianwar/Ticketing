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
    public class TicketAttachementsRepository : ICommanRepository<TicketAttachements>
    {
        private readonly ApplicationDbContext _context;

        public TicketAttachementsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        

        async Task<TicketAttachements> ICommanRepository<TicketAttachements>.Create(TicketAttachements TicketAttachements)
        {
            
            try
            {
                _context.TicketAttachements.Add(TicketAttachements);
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {

            }
            return TicketAttachements;
        }

        async Task ICommanRepository<TicketAttachements>.DeleteConfirmed(string id)
        {
            var TicketAttachements = await _context.TicketAttachements.FindAsync(id);
            _context.TicketAttachements.Remove(TicketAttachements);
            await _context.SaveChangesAsync();
        }

        async Task<TicketAttachements> ICommanRepository<TicketAttachements>.Details(string id)
        {

            return await _context.TicketAttachements.FindAsync(id);
        }

        async Task ICommanRepository<TicketAttachements>.Edit(TicketAttachements TicketAttachements)
        {
            _context.Entry(TicketAttachements).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

       async Task<IEnumerable<TicketAttachements>> ICommanRepository<TicketAttachements>.Index()
        {
            return await _context.TicketAttachements.ToListAsync();
        }

        async Task<IEnumerable<TicketAttachements>> ICommanRepository<TicketAttachements>.Search(TicketAttachements ticketComman)
        {
            return await _context.TicketAttachements
                    .Where(b => b.TicketId == ticketComman.TicketId).ToListAsync();
        }
        public async Task<IEnumerable<TicketAttachements>> SearchVM(TicketVM ticketComman)
        {
            return await _context.TicketAttachements
                    .Where(b => b.TicketId == ticketComman.TicketId).ToListAsync();
        }
    }
}
