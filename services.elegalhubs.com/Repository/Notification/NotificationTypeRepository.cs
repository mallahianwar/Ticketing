using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using library.elegalhubs.com.Notification;
using Microserver_publisher.Services;
using Microsoft.EntityFrameworkCore;
using www.service.com.Data;

namespace services.elegalhubs.com.Repository.Notification
{
    public class NotificationTypeRepository :ICommanRepository<NotificationTypes>
    {
        private readonly ApplicationDbContext _context;

        public NotificationTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<NotificationTypes> Search(NotificationTypes ticketComman)
        {
            throw new NotImplementedException();
        }

        async Task<NotificationTypes> ICommanRepository<NotificationTypes>.Create(NotificationTypes notifications)
        {
            _context.NotificationTypes.Add(notifications);
            await _context.SaveChangesAsync();
            return notifications;
        }

        async Task ICommanRepository<NotificationTypes>.DeleteConfirmed(string id)
        {
            var notification = await _context.NotificationTypes.FindAsync(id);
            _context.NotificationTypes.Remove(notification);
            await _context.SaveChangesAsync();
        }

        async Task<NotificationTypes> ICommanRepository<NotificationTypes>.Details(string id)
        {
            return await _context.NotificationTypes.FindAsync(id);
        }

        async Task ICommanRepository<NotificationTypes>.Edit(NotificationTypes notifications)
        {
            _context.Entry(notifications).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        async Task<IEnumerable<NotificationTypes>> ICommanRepository<NotificationTypes>.Index()
        {
            return await _context.NotificationTypes.ToListAsync();
        }

        Task<IEnumerable<NotificationTypes>> ICommanRepository<NotificationTypes>.Search(NotificationTypes ticketComman)
        {
            throw new NotImplementedException();
        }
    }
}
