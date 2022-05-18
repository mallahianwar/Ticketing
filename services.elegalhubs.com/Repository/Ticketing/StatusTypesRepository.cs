using library.elegalhubs.com.Ticketing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using www.service.com.Data;

namespace services.elegalhubs.com.Repository
{
    public class StatusTypesRepository : ICommanRepository<StatusTypes>
    {
        private readonly ApplicationDbContext _context;

        public StatusTypesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<StatusTypes> Search(StatusTypes ticketComman)
        {
            throw new NotImplementedException();
        }

        async Task<StatusTypes> ICommanRepository<StatusTypes>.Create(StatusTypes statusTypes)
        {
            _context.StatusTypes.Add(statusTypes);
            await _context.SaveChangesAsync();
            return statusTypes;
        }

        async Task ICommanRepository<StatusTypes>.DeleteConfirmed(string id)
        {
            var statusTypes = await _context.StatusTypes.FindAsync(id);
            _context.StatusTypes.Remove(statusTypes);
            await _context.SaveChangesAsync();
        }

        async Task<StatusTypes> ICommanRepository<StatusTypes>.Details(string id)
        {

            return await _context.StatusTypes.FindAsync(id);
        }

        async Task ICommanRepository<StatusTypes>.Edit(StatusTypes statusTypes)
        {
            _context.Entry(statusTypes).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

       async Task<IEnumerable<StatusTypes>> ICommanRepository<StatusTypes>.Index()
        {
            return await _context.StatusTypes.ToListAsync();
        }

        Task<IEnumerable<StatusTypes>> ICommanRepository<StatusTypes>.Search(StatusTypes ticketComman)
        {
            throw new NotImplementedException();
        }
    }
}
