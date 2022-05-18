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
    public class TicketDetailsRepository : ICommanRepository<TicketDetails>
    {
        private readonly ApplicationDbContext _context;

        public TicketDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<TicketDetails>> Search(TicketDetails ticketComman)
        {
            throw new NotImplementedException();
        }

        async Task<TicketDetails> ICommanRepository<TicketDetails>.Create(TicketDetails ticketDetails)
        {
            _context.TicketDetails.Add(ticketDetails);
            await _context.SaveChangesAsync();
            return ticketDetails;
        }

        async Task ICommanRepository<TicketDetails>.DeleteConfirmed(string id)
        {
            var ticketDetails = await _context.TicketDetails.FindAsync(id);
            _context.TicketDetails.Remove(ticketDetails);
            await _context.SaveChangesAsync();
        }

        async Task<TicketDetails> ICommanRepository<TicketDetails>.Details(string id)
        {

            return await _context.TicketDetails.FindAsync(id);
        }

        async Task ICommanRepository<TicketDetails>.Edit(TicketDetails ticketDetails)
        {
            _context.Entry(ticketDetails).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

       async Task<IEnumerable<TicketDetails>> ICommanRepository<TicketDetails>.Index()
        {
            return await _context.TicketDetails.ToListAsync();
        }

        async Task<IEnumerable<TicketDetails>> ICommanRepository<TicketDetails>.Search(TicketDetails ticketComman)
        {
            return await _context.TicketDetails
                    .Where(b => b.TicketId == ticketComman.TicketId).ToListAsync();
        }


        public async Task<IEnumerable<TicketDetails>> SearchVM(TicketVM ticketComman)
        {
            return await _context.TicketDetails
                    .Where(b => b.TicketId == ticketComman.TicketId).ToListAsync();
        }
    }
}
