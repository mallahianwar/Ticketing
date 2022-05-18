using library.elegalhubs.com.Ticketing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using www.service.com.Data;

namespace services.elegalhubs.com.Repository
{
    public class ContactTypesRepository : ICommanRepository<ContactTypes>
    {
        private readonly ApplicationDbContext _context;

        public ContactTypesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<ContactTypes> Search(ContactTypes ticketComman)
        {
            throw new NotImplementedException();
        }

        async Task<ContactTypes> ICommanRepository<ContactTypes>.Create(ContactTypes contactTypes)
        {
            _context.ContactTypes.Add(contactTypes);
            await _context.SaveChangesAsync();
            return contactTypes;
        }

        async Task ICommanRepository<ContactTypes>.DeleteConfirmed(string id)
        {
            var contactTypes = await _context.ContactTypes.FindAsync(id);
            _context.ContactTypes.Remove(contactTypes);
            await _context.SaveChangesAsync();
        }

        async Task<ContactTypes> ICommanRepository<ContactTypes>.Details(string id)
        {

            return await _context.ContactTypes.FindAsync(id);
        }

        async Task ICommanRepository<ContactTypes>.Edit(ContactTypes contactTypes)
        {
            _context.Entry(contactTypes).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

       async Task<IEnumerable<ContactTypes>> ICommanRepository<ContactTypes>.Index()
        {
            return await _context.ContactTypes.ToListAsync();
        }

        Task<IEnumerable<ContactTypes>> ICommanRepository<ContactTypes>.Search(ContactTypes ticketComman)
        {
            throw new NotImplementedException();
        }
    }
}


