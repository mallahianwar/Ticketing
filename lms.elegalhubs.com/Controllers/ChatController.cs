using library.elegalhubs.com.lms.Admin;
using lms.elegalhubs.com.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lms.elegalhubs.com.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<Users> _userManager;
        public ChatController(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }
        //DataLayer dl = new DataLayer();
        // GET: Home  
        public ActionResult Index()
        {
            //if (Session["userid"] == null)
            //{
            //    return RedirectToAction("login");
            //}
            //else
            //{
                return View();
            //}
        }
        [HttpPost]
        public async Task<JsonResult> sendmsg(string message, string friend)
        {
            //RabbitMQBll obj = new RabbitMQBll();
            IConnection con = RabbitMQBll.GetConnection();
            bool flag = await RabbitMQBll.send(con, message, friend);
            return Json(null);
        }
        [HttpPost]
        public async Task<JsonResult> receive()
        {
            try
            {
                //RabbitMQBll obj = new RabbitMQBll();
                IConnection con = RabbitMQBll.GetConnection();
                //string userqueue = Session["username"].ToString();
                string userqueue = "";
                string message = await RabbitMQBll.receive(con, userqueue);
                return Json(message);
            }
            catch (Exception)
            {

                return null;
            }


        }
        public ActionResult login()
        {

            return View();
        }
        //[HttpPost]
        //public ActionResult login(FormCollection fc)
        //{
        //    string email = fc["txtemail"].ToString();
        //    string password = fc["txtpassword"].ToString();
        //    UserModel user = dl.login(email, password);
        //    if (user.userid > 0)
        //    {
        //        ViewData["status"] = 1;
        //        ViewData["msg"] = "login Successful...";
        //        Session["username"] = user.email;
        //        Session["userid"] = user.userid.ToString();
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {

        //        ViewData["status"] = 2;
        //        ViewData["msg"] = "invalid Email or Password...";
        //        return View();
        //    }

        //}

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
