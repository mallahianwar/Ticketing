using library.elegalhubs.com.Ticketing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using www.service.com.Data;

namespace services.elegalhubs.com.Repository
{
    public class TicketTypeRepository : ICommanRepository<TicketTypes>
    {
        private readonly ApplicationDbContext _context;

        public TicketTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task <IEnumerable<TicketTypes>> Search(TicketTypes ticketComman)
        {
            throw new NotImplementedException();
        }

        async Task<TicketTypes> ICommanRepository<TicketTypes>.Create(TicketTypes ticketTypes)
        {
            _context.TicketTypes.Add(ticketTypes);
            await _context.SaveChangesAsync();
            return ticketTypes;
        }

        async Task ICommanRepository<TicketTypes>.DeleteConfirmed(string id)
        {
            var ticketTypes = await _context.TicketTypes.FindAsync(id);
            _context.TicketTypes.Remove(ticketTypes);
            await _context.SaveChangesAsync();
        }

        async Task<TicketTypes> ICommanRepository<TicketTypes>.Details(string id)
        {

            return await _context.TicketTypes.FindAsync(id);
        }

        async Task ICommanRepository<TicketTypes>.Edit(TicketTypes ticketTypes)
        {
            _context.Entry(ticketTypes).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

       async Task<IEnumerable<TicketTypes>> ICommanRepository<TicketTypes>.Index()
        {
            return await _context.TicketTypes.ToListAsync();
        }
    }
}
