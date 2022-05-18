using library.elegalhubs.com.lms.Admin;
using library.elegalhubs.com.lms.Admin.Reports;
using lms.elegalhubs.com.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace lms.elegalhubs.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {


        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;
        private readonly UserManager<Users> _userManager;

        public HomeController(ApplicationDbContext context, IConfiguration configuration, UserManager<Users> userManager)
        {
            _context = context;
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (await _userManager.IsInRoleAsync(_userManager.GetUserAsync(User).Result,"SuperAdmin")) {
                var currentUser = _userManager.GetUserAsync(HttpContext.User).Result.Id;

                IndexReport data = new IndexReport();
                using (HttpClient client = new HttpClient())
                {
                    string endpoint = apiBaseUrl + "/Reports/IndexReport";

                    using (var Response = await client.GetAsync(endpoint))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string applicationDbContext1 = await Response.Content.ReadAsStringAsync();
                            data = JsonConvert.DeserializeObject<IndexReport>(applicationDbContext1);
                            data.UsersMessages = await _context.UsersMessages.Where(m => m.Reciver.Id == currentUser).OrderByDescending(r => r.DateTime)
                                            .Include(r => r.Sender).Take(5).ToListAsync();
                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                        }
                    }
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("IndexEmp");
            }

        }
        public async Task<IActionResult> IndexEmp()
        {
            return View();
        }
    }
}
