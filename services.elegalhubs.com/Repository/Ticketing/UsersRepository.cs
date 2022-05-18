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
    public class UsersRepository : ICommanRepository<User>
    {
        private readonly ApplicationDbContext _context;

        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<User> Details(string id)
        {
            throw new NotImplementedException();
        }

        
        async Task<IEnumerable<User>> ICommanRepository<User>.Search(User user)
        {
            var usr = await _context.User
                    .Where(b => b.UserId == user.UserId && b.ProjectId == user.ProjectId).ToListAsync();
            return usr;
        }
        async Task<User> ICommanRepository<User>.Create(User user)
        {
            try
            {
                var usr = await _context.User
                    .Where(b => b.UserId == user.UserId && b.ProjectId == user.ProjectId).FirstOrDefaultAsync();
                if (usr == null)
                {
                    _context.User.Add(user);
                    await _context.SaveChangesAsync();
                }
                else
                    return usr;
                    
            }
            catch(Exception ex)
            {

            }
            return user;
        }

        async Task ICommanRepository<User>.DeleteConfirmed(string id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
        }

        async Task<User> ICommanRepository<User>.Details(string id)
        {

            return await _context.User.FindAsync(id);
        }
        

        async Task ICommanRepository<User>.Edit(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

       async Task<IEnumerable<User>> ICommanRepository<User>.Index()
        {
            return await _context.User
                .Include(m => m.Projects)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }
        public async Task<IEnumerable<User>> SearchVM(UserVM user)
        {
            var usr = await _context.User
                    .Where(b => b.UserId == user.UserId && b.ProjectId == user.ProjectId).ToListAsync();
            return usr;
        }
    }
}
