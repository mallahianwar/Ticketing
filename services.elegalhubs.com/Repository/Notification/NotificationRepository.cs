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
    public class NotificationRepository : ICommanRepository<Notifications>
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Notifications> Search(Notifications ticketComman)
        {
            throw new NotImplementedException();
        }

        async Task<Notifications> ICommanRepository<Notifications>.Create(Notifications notifications)
        {
            _context.Notifications.Add(notifications);
            try
            {
                //NotificationService.RabbitNotification(notifications);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex) {

            }
            return notifications;
        }

        async Task ICommanRepository<Notifications>.DeleteConfirmed(string id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }

        async Task<Notifications> ICommanRepository<Notifications>.Details(string id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        async Task ICommanRepository<Notifications>.Edit(Notifications notifications)
        {
            _context.Entry(notifications).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        async Task<IEnumerable<Notifications>> ICommanRepository<Notifications>.Index()
        {
            return await _context.Notifications.ToListAsync();
        }

        Task<IEnumerable<Notifications>> ICommanRepository<Notifications>.Search(Notifications ticketComman)
        {
            throw new NotImplementedException();
        }
    }
}
