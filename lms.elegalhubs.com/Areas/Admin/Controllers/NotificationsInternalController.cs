using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lms.elegalhubs.com.Areas.Admin.Models;
using lms.elegalhubs.com.Data;
using lms.elegalhubs.com.Helpers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using library.elegalhubs.com.lms.Admin;
using Microsoft.AspNetCore.Authorization;

namespace lms.elegalhubs.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class NotificationsInternalController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly UserManager<Users> _userManager;

        public NotificationsInternalController(UserManager<Users> userManager, ApplicationDbContext context, IHubContext<MessageHub> hubContext)
        {
            _userManager = userManager;
            _context = context;
            _hubContext = hubContext;
        }

        // GET: Admin/NotificationsInternal
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Notifications.Where(p=>p.UserTo == _userManager.GetUserAsync(User).Result.Id).OrderByDescending(p=>p.ActivityDate).Include(n => n.NotificationTypes).Include(n => n.Reciver).Include(n => n.Sender);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/NotificationsInternal/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notifications = await _context.Notifications
                .Include(n => n.NotificationTypes)
                .Include(n => n.Reciver)
                .Include(n => n.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notifications == null)
            {
                return NotFound();
            }

            return View(notifications);
        }

        // GET: Admin/NotificationsInternal/Create
        public async Task<IActionResult> Create()
        {
            ViewData["NotificationTypeId"] = new SelectList(_context.Set<NotificationTypes>(), "Id", "NotificationType");
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();

            ViewData["UserTo"] = new SelectList(allUsersExceptCurrentUser, "Id", "FullName");
            //ViewData["UserFrom"] = new SelectList(_context.Users, "Id", "FullName");
            return View();
        }

        // POST: Admin/NotificationsInternal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Module,NotificationTypeId,Status,Purpose,Subject,ActivityDate,Message,UserFrom,UserTo")] Notifications notifications)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (ModelState.IsValid)
            {
                try
                {
                    notifications.NotificationTypeId = "1";
                    notifications.UserFrom = currentUser.Id;
                    _context.Add(notifications);
                    await _context.SaveChangesAsync();
                    //MessageHub msg = new MessageHub();
                    //var d = _context.Users.FirstOrDefault(m => m.Id == notifications.UserTo).UserName; //(l => l.Id == notifications.Id).UserName;
                    //await msg.SendMessage(d, notifications.Message);
                    //await _hubContext.Clients.All.SendAsync("ReceiveMessage", "the weatherman", $" The temperature will be {weatherForecastModels[0].TemperatureC}").ConfigureAwait(false);
                    //var connectionID = msg.GetConnectionId();

                    if (string.IsNullOrEmpty(notifications.UserTo))
                        await _hubContext.Clients.All.SendAsync("ReceiveMessageHandler", notifications.Message);
                    else
                        await _hubContext.Clients.User(notifications.UserTo).SendAsync("ReceiveMessageHandler", notifications.Message);


                    return View();
                }catch (Exception ex)
                {
                   
                }
            }
            ViewData["NotificationTypeId"] = new SelectList(_context.Set<NotificationTypes>(), "Id", "Id", notifications.NotificationTypeId);
            ViewData["UserTo"] = new SelectList(_context.Users, "Id", "Id", notifications.UserTo);
            ViewData["UserFrom"] = new SelectList(_context.Users, "Id", "Id", notifications.UserFrom);
            return View(notifications);
        }

        // GET: Admin/NotificationsInternal/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notifications = await _context.Notifications.FindAsync(id);
            if (notifications == null)
            {
                return NotFound();
            }
            ViewData["NotificationTypeId"] = new SelectList(_context.Set<NotificationTypes>(), "Id", "Id", notifications.NotificationTypeId);
            ViewData["UserTo"] = new SelectList(_context.Users, "Id", "Id", notifications.UserTo);
            ViewData["UserFrom"] = new SelectList(_context.Users, "Id", "Id", notifications.UserFrom);
            return View(notifications);
        }

        // POST: Admin/NotificationsInternal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Module,NotificationTypeId,Status,Purpose,Subject,ActivityDate,Message,UserFrom,UserTo")] Notifications notifications)
        {
            if (id != notifications.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notifications);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationsExists(notifications.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["NotificationTypeId"] = new SelectList(_context.Set<NotificationTypes>(), "Id", "Id", notifications.NotificationTypeId);
            ViewData["UserTo"] = new SelectList(_context.Users, "Id", "Id", notifications.UserTo);
            ViewData["UserFrom"] = new SelectList(_context.Users, "Id", "Id", notifications.UserFrom);
            return View(notifications);
        }

        // GET: Admin/NotificationsInternal/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notifications = await _context.Notifications
                .Include(n => n.NotificationTypes)
                .Include(n => n.Reciver)
                .Include(n => n.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notifications == null)
            {
                return NotFound();
            }

            return View(notifications);
        }

        // POST: Admin/NotificationsInternal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var notifications = await _context.Notifications.FindAsync(id);
            _context.Notifications.Remove(notifications);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationsExists(string id)
        {
            return _context.Notifications.Any(e => e.Id == id);
        }
    }
}
