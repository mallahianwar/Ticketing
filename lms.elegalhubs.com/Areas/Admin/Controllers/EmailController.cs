using lms.elegalhubs.com.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lms.elegalhubs.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class EmailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string Subject,string Msg , string To)
        {
            var result = MailHelper.SendEmail(Subject, To, Msg);
            if (!result)
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "Error ! Email not sended , please try again late");
            }
            return RedirectToAction("Index");
        }
    }
}
