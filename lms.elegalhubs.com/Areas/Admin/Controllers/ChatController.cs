using library.elegalhubs.com.lms.Admin;
using library.elegalhubs.com.lms.Admin.Chat;
using lms.elegalhubs.com.Data;
using lms.elegalhubs.com.Enums;
using lms.elegalhubs.com.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lms.elegalhubs.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly ApplicationDbContext _context;
        public ChatController(UserManager<Users> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Drawerchat()
        {
            return View();
        }
        public IActionResult Groupchat()
        {
            return View();
        }
        public async Task<IActionResult> Privatechat(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();
            if (userId != null)
            {
                ViewBag.User = await _userManager.Users.Where(a => a.Id == userId).FirstOrDefaultAsync();
            }
            else
            {
                if (allUsersExceptCurrentUser.Count > 0)
                {
                    userId = allUsersExceptCurrentUser[0].Id;
                    ViewBag.User = allUsersExceptCurrentUser[0];
                }
            }
            
            if(userId != null)
            {
                ViewBag.crntusr = currentUser.Id;
                var Messages = await _context.UsersMessages.ToListAsync();
                var queryMsgs = from msg in Messages
                                where (msg.UserFrom == userId && msg.UserTo == currentUser.Id) || (msg.UserFrom == currentUser.Id && msg.UserTo == userId)
                                select msg;
                ViewBag.Messages = queryMsgs.OrderBy(a =>a.DateTime);
            }
            return View(allUsersExceptCurrentUser);
        }
        public async Task<IActionResult> InternalChat(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();
            if (userId != null)
            {
                ViewBag.User = await _userManager.Users.Where(a => a.Id == userId).FirstOrDefaultAsync();
            }
            else
            {
                if (allUsersExceptCurrentUser.Count > 0)
                {
                    userId = allUsersExceptCurrentUser[0].Id;
                    ViewBag.User = allUsersExceptCurrentUser[0];
                }
            }
            
            if(userId != null)
            {
                ViewBag.crntusr = currentUser.Id;
                var Messages = await _context.UsersMessages.ToListAsync();
                var queryMsgs = from msg in Messages
                                where (msg.UserFrom == userId && msg.UserTo == currentUser.Id) || (msg.UserFrom == currentUser.Id && msg.UserTo == userId)
                                select msg;
                ViewBag.Messages = queryMsgs.OrderBy(a =>a.DateTime);
            }
            return View(allUsersExceptCurrentUser);
        }
        [HttpPost]
        public async Task<JsonResult> sendmsg(string message, string friend)
        {
            //RabbitMQBll obj = new RabbitMQBll();
            //IConnection con = RabbitMQBll.GetConnection();
            //bool flag = await RabbitMQBll.send(con, message, friend);
            //if (flag)
            //{
                var msg = new UsersMessages
                {
                    UserFrom = _userManager.GetUserAsync(HttpContext.User).Result.Id,
                    UserTo = friend,
                    Content = message
                };
                try
                {
                   await _context.AddAsync(msg);
                   await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                   
                }
            //}
                
            return Json(null);
        }
        [HttpPost]
        public async Task<JsonResult> receive()
        {
            try
            {
                IConnection con = RabbitMQBll.GetConnection();
                string userqueue =  _userManager.GetUserAsync(HttpContext.User).Result.Id;
                string message = await RabbitMQBll.receive(con, userqueue);
                return Json(message);
            }
            catch (Exception)
            {
                return null;
            }
        }
        [HttpPost]
        public async Task<JsonResult> friendlist()
        {
            //int id = Convert.ToInt32(Session["userid"].ToString());
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();

            //List<UserModel> users = dl.getusers(id);
            //List<ListItem> userlist = new List<ListItem>();
            //foreach (var item in users)
            //{
            //    userlist.Add(new ListItem
            //    {
            //        Value = item.email.ToString(),
            //        Text = item.email.ToString()

            //    });
            //}
            return Json(allUsersExceptCurrentUser);
        }
    }
}
