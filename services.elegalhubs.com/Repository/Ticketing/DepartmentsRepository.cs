using library.elegalhubs.com.Ticketing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using www.service.com.Data;

namespace services.elegalhubs.com.Repository
{
    public class DepartmentsRepository : ICommanRepository<Departments>
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        async Task<Departments> ICommanRepository<Departments>.Create(Departments department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        async Task ICommanRepository<Departments>.DeleteConfirmed(string id)
        {
            var departments = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(departments);
            await _context.SaveChangesAsync();
        }

        async Task<Departments> ICommanRepository<Departments>.Details(string id)
        {

            return await _context.Departments.FindAsync(id);
        }

        async Task ICommanRepository<Departments>.Edit(Departments departments)
        {
            _context.Entry(departments).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

       async Task<IEnumerable<Departments>> ICommanRepository<Departments>.Index()
        {
            return await _context.Departments
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        Task<IEnumerable<Departments>> ICommanRepository<Departments>.Search(Departments ticketComman)
        {
            throw new NotImplementedException();
        }
    }
}
