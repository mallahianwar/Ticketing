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
    public class ProjectsRepository : ICommanRepository<Projects>
    {
        private readonly ApplicationDbContext _context;

        public ProjectsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Projects> Search(Projects ticketComman)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TicketDetails>> Search(TicketVM ticketComman)
        {
            throw new NotImplementedException();
        }

        async Task<Projects> ICommanRepository<Projects>.Create(Projects projects)
        {
            _context.Projects.Add(projects);
            await _context.SaveChangesAsync();
            return projects;
        }

        async Task ICommanRepository<Projects>.DeleteConfirmed(string id)
        {
            var projects = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(projects);
            await _context.SaveChangesAsync();
        }

        async Task<Projects> ICommanRepository<Projects>.Details(string id)
        {

            return await _context.Projects.FindAsync(id);
        }

        async Task ICommanRepository<Projects>.Edit(Projects projects)
        {
            _context.Entry(projects).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

       async Task<IEnumerable<Projects>> ICommanRepository<Projects>.Index()
        {
            return await _context.Projects
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        Task<IEnumerable<Projects>> ICommanRepository<Projects>.Search(Projects ticketComman)
        {
            throw new NotImplementedException();
        }
    }
}
