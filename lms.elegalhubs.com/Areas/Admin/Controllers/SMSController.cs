//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace lms.elegalhubs.com.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class SMSController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//        [HttpPost]
//        public IActionResult Create(string Msg, string To,string From)
//        {
//            TwilioClient m = new TwilioClient();
//            m.sendSMS(From, To, Msg);
//            return View("Index");
//        }
//    }
//}
