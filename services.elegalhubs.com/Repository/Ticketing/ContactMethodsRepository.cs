using library.elegalhubs.com.Ticketing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using www.service.com.Data;

namespace services.elegalhubs.com.Repository
{
    public class ContactMethodsRepository : ICommanRepository<ContactMethods>
    {
        private readonly ApplicationDbContext _context;

        public ContactMethodsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<ContactMethods> Search(ContactMethods ticketComman)
        {
            throw new NotImplementedException();
        }

        async Task<ContactMethods> ICommanRepository<ContactMethods>.Create(ContactMethods contactMethods)
        {
            _context.ContactMethods.Add(contactMethods);
            await _context.SaveChangesAsync();
            return contactMethods;
        }

        async Task ICommanRepository<ContactMethods>.DeleteConfirmed(string id)
        {
            var contactMethods = await _context.ContactMethods.FindAsync(id);
            _context.ContactMethods.Remove(contactMethods);
            await _context.SaveChangesAsync();
        }

        async Task<ContactMethods> ICommanRepository<ContactMethods>.Details(string id)
        {

            return await _context.ContactMethods.FindAsync(id);
        }

        async Task ICommanRepository<ContactMethods>.Edit(ContactMethods contactMethods)
        {
            _context.Entry(contactMethods).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

       async Task<IEnumerable<ContactMethods>> ICommanRepository<ContactMethods>.Index()
        {
            return await _context.ContactMethods.ToListAsync();
        }

        Task<IEnumerable<ContactMethods>> ICommanRepository<ContactMethods>.Search(ContactMethods ticketComman)
        {
            throw new NotImplementedException();
        }
    }
}
