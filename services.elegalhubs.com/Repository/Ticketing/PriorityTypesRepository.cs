using library.elegalhubs.com.Ticketing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using www.service.com.Data;

namespace services.elegalhubs.com.Repository
{
    public class PriorityTypesRepository : ICommanRepository<PriorityTypes>
    {
        private readonly ApplicationDbContext _context;

        public PriorityTypesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<PriorityTypes> Search(PriorityTypes ticketComman)
        {
            throw new NotImplementedException();
        }

        async Task<PriorityTypes> ICommanRepository<PriorityTypes>.Create(PriorityTypes priorityTypes)
        {
            _context.PriorityTypes.Add(priorityTypes);
            await _context.SaveChangesAsync();
            return priorityTypes;
        }

        async Task ICommanRepository<PriorityTypes>.DeleteConfirmed(string id)
        {
            var priorityTypes = await _context.PriorityTypes.FindAsync(id);
            _context.PriorityTypes.Remove(priorityTypes);
            await _context.SaveChangesAsync();
        }

        async Task<PriorityTypes> ICommanRepository<PriorityTypes>.Details(string id)
        {

            return await _context.PriorityTypes.FindAsync(id);
        }

        async Task ICommanRepository<PriorityTypes>.Edit(PriorityTypes priorityTypes)
        {
            _context.Entry(priorityTypes).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

       async Task<IEnumerable<PriorityTypes>> ICommanRepository<PriorityTypes>.Index()
        {
            return await _context.PriorityTypes.ToListAsync();
        }

        Task<IEnumerable<PriorityTypes>> ICommanRepository<PriorityTypes>.Search(PriorityTypes ticketComman)
        {
            throw new NotImplementedException();
        }
    }
}
