using library.elegalhubs.com.Ticketing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using www.service.com.Data;

namespace services.elegalhubs.com.Repository
{
    public class CategoryTypesRepository : ICommanRepository<CategoryTypes>
    {
        private readonly ApplicationDbContext _context;

        public CategoryTypesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<CategoryTypes> Search(CategoryTypes ticketComman)
        {
            throw new NotImplementedException();
        }

        async Task<CategoryTypes> ICommanRepository<CategoryTypes>.Create(CategoryTypes categoryTypes)
        {
            _context.CategoryTypes.Add(categoryTypes);
            await _context.SaveChangesAsync();
            return categoryTypes;
        }



        async Task ICommanRepository<CategoryTypes>.DeleteConfirmed(string id)
        {
            var categoryTypes = await _context.CategoryTypes.FindAsync(id);
            _context.CategoryTypes.Remove(categoryTypes);
            await _context.SaveChangesAsync();
        }

        async Task<CategoryTypes> ICommanRepository<CategoryTypes>.Details(string id)
        {

            return await _context.CategoryTypes.FindAsync(id);
        }

        async Task ICommanRepository<CategoryTypes>.Edit(CategoryTypes categoryTypes)
        {
            _context.Entry(categoryTypes).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

       async Task<IEnumerable<CategoryTypes>> ICommanRepository<CategoryTypes>.Index()
        {
            return await _context.CategoryTypes
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        Task<IEnumerable<CategoryTypes>> ICommanRepository<CategoryTypes>.Search(CategoryTypes ticketComman)
        {
            throw new NotImplementedException();
        }
    }
}
