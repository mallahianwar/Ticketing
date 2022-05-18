using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using services.elegalhubs.com.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace services.elegalhubs.com.Controllers.Chat
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> sendmsg(string message, string FromUserId,string ToUserId, string Project)
        {
            RabbitMQBll obj = new RabbitMQBll();
            IConnection con = obj.GetConnection();
            bool flag = await obj.Send(con, message, ToUserId);
            if (flag)
            {
                return Json(new { msg = "done successfuly" });
            }
            return Json(new { msg = "Error : Please try again" });
        }
        [HttpPost]
        public async Task<JsonResult> receive(string ToUserId)
        {
            try
            {
                RabbitMQBll obj = new RabbitMQBll();
                IConnection con = obj.GetConnection();
                string message = await obj.Receive(con, ToUserId);
                return Json(new { message = message , FromUser = "" });
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
