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

namespace lms.elegalhubs.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;
        public DepartmentsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }


        // GET: Admin/Departments
        public async Task<IActionResult> Index()
        {
            List<Departments> DepartmentsList = new List<Departments>();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/Departments";

                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string applicationDbContext1 = await Response.Content.ReadAsStringAsync();
                        DepartmentsList = JsonConvert.DeserializeObject<List<Departments>>(applicationDbContext1);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                    }
                }
            }
            return View(DepartmentsList);
        }

        //GET: Admin/Departments/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var Departments = await _context.Departments
            //    .FirstOrDefaultAsync(m => m.Id == id);



            Departments Departments = new Departments();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/Departments/" + id;

                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string applicationDbContext1 = await Response.Content.ReadAsStringAsync();
                        Departments = JsonConvert.DeserializeObject<Departments>(applicationDbContext1);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                    }
                }
            }


            if (Departments == null)
            {
                return NotFound();
            }

            return View(Departments);
        }

        // GET: Admin/Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Departments Departments)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            Departments receivedProject = new Departments();
            string endpoint = apiBaseUrl + "/Departments";
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(Departments), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(endpoint, content))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            receivedProject = JsonConvert.DeserializeObject<Departments>(apiResponse);
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

        // GET: Admin/Departments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Departments project = new Departments();
            string endpoint = apiBaseUrl + "/Departments/";

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(endpoint + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        project = JsonConvert.DeserializeObject<Departments>(apiResponse);
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
                return NotFound();
            }
            return View(project);
        }

        // POST: Admin/Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] Departments Departments)
        {
            if (id != Departments.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                    string endpoint = apiBaseUrl + "/Departments/" + id;
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(Departments), Encoding.UTF8, "application/json");

                        using (var response = await httpClient.PutAsync(endpoint, content))
                        {
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();

                                if (isAjax)
                                    if (!string.IsNullOrEmpty(apiResponse))
                                        return Json(new { message = "Error ! Please try again .", color = "error" });
                                    else
                                        return Json(new { message = "done successfuly !", color = "success" });
                                return RedirectToAction(nameof(Index));
                                //receivedProject = JsonConvert.DeserializeObject<Departments>(apiResponse);
                            }
                            else
                            {
                                ModelState.Clear();
                                ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                            }
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentsExists(Departments.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View(Departments);
            }
            return View(Departments);
        }

        // POST: Admin/Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            string endpoint = apiBaseUrl + "/Departments/";
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(endpoint + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (isAjax)
                            if (apiResponse == "")
                                return Json(new { message = "done successfuly !", color = "success" });
                            else
                                return Json(new { message = "Error ! Please try again .", color = "error" });
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                        return View("Index", new Departments());
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentsExists(string id)
        {
            Departments project = new Departments();
            string endpoint = apiBaseUrl + "/Departments/";

            using (var httpClient = new HttpClient())
            {
                using (var response = httpClient.GetAsync(endpoint + id))
                {
                    if (response.IsCompletedSuccessfully)
                    {
                        //string apiResponse =  response.Content.ReadAsStringAsync();
                        string apiResponse = response.Result.Content.ToString();
                        project = JsonConvert.DeserializeObject<Departments>(apiResponse);
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
