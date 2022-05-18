using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using library.elegalhubs.com.Ticketing;
using lms.elegalhubs.com.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using library.elegalhubs.com.Notification;
using lms.elegalhubs.com.Helpers;
using Microsoft.AspNetCore.Identity;
using library.elegalhubs.com.lms.Admin;

namespace lms.elegalhubs.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class Notifications11Controller : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;
        private readonly UserManager<Users> _userManager;

        public Notifications11Controller(ApplicationDbContext context, IConfiguration configuration, UserManager<Users> userManager)
        {
            _context = context;
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
            _userManager = userManager;
        }


        // GET: Admin/Notifications
        public async Task<IActionResult> Index()
        {
            List<Notifications> NotificationsList = new List<Notifications>();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/Notifications";

                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string applicationDbContext1 = await Response.Content.ReadAsStringAsync();
                        NotificationsList = JsonConvert.DeserializeObject<List<Notifications>>(applicationDbContext1);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                    }
                }
            }
            return View(NotificationsList);
        }

        //GET: Admin/Notifications/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var Notifications = await _context.Notifications
            //    .FirstOrDefaultAsync(m => m.Id == id);



            Notifications Notifications = new Notifications();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/Notifications/" + id;

                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string applicationDbContext1 = await Response.Content.ReadAsStringAsync();
                        Notifications = JsonConvert.DeserializeObject<Notifications>(applicationDbContext1);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                    }
                }
            }


            if (Notifications == null)
            {
                return NotFound();
            }

            return View(Notifications);
        }

        // GET: Admin/Notifications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Notifications Notifications)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            Notifications.NotificationTypeId = "1";
            Notifications.UserFrom = _userManager.GetUserAsync(HttpContext.User).Result.Id;

            Notifications receivedProject = new Notifications();
            string endpoint = apiBaseUrl + "/Notifications";
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(Notifications), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(endpoint, content))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            MessageHub msg = new MessageHub();
                            msg.SendMessage("superadmin@gmail.com", Notifications.Message);
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            receivedProject = JsonConvert.DeserializeObject<Notifications>(apiResponse);
                            if (isAjax)
                                if (string.IsNullOrEmpty(receivedProject.Id))
                                    return Json(new { message = "Error ! Please try again .", color = "error" });
                                else
                                    return Json(new { message = "done successfuly !", color = "success" });
                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                        }

                    }
                }

            }
            return View(receivedProject);
        }

   
        private bool NotificationsExists(string id)
        {
            Notifications project = new Notifications();
            string endpoint = apiBaseUrl + "/Notifications/";

            using (var httpClient = new HttpClient())
            {
                using (var response = httpClient.GetAsync(endpoint + id))
                {
                    if (response.IsCompletedSuccessfully)
                    {
                        //string apiResponse =  response.Content.ReadAsStringAsync();
                        string apiResponse = response.Result.Content.ToString();
                        project = JsonConvert.DeserializeObject<Notifications>(apiResponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                    }
                }
            }

            if (project == null)
            {
                return false;
            }

            return project.Id == null ? false : true;
        }
    }
}
