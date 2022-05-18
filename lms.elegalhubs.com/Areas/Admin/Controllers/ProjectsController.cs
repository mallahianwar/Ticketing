using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using library.elegalhubs.com.Ticketing;
using lms.elegalhubs.com.Data;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace lms.elegalhubs.com.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Area("Admin")]
    public class ProjectsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;
        public ProjectsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }


        // GET: Admin/Projects
        public async Task<IActionResult> Index()
        {
            List<Projects> projectsList = new List<Projects>();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/Projects";

                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string applicationDbContext1 = await Response.Content.ReadAsStringAsync();
                        projectsList = JsonConvert.DeserializeObject<List<Projects>>(applicationDbContext1);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                    }
                }
            }
            return View(projectsList);
        }

        //GET: Admin/Projects/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var projects = await _context.Projects
            //    .FirstOrDefaultAsync(m => m.Id == id);



            Projects projects = new Projects();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/Projects/"+id;

                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string applicationDbContext1 = await Response.Content.ReadAsStringAsync();
                        projects = JsonConvert.DeserializeObject<Projects>(applicationDbContext1);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                    }
                }
            }


            if (projects == null)
            {
                return NotFound();
            }

            return View(projects);
        }

        // GET: Admin/Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,APIEndPoint,UserName,Password")] Projects projects)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            Projects receivedProject = new Projects();
            string endpoint = apiBaseUrl + "/Projects";
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(projects), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(endpoint, content))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            receivedProject = JsonConvert.DeserializeObject<Projects>(apiResponse);
                            if (isAjax)
                                if(string.IsNullOrEmpty(receivedProject.Id))
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

        // GET: Admin/Projects/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Projects project = new Projects();
            string endpoint = apiBaseUrl + "/Projects/";

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(endpoint + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        project = JsonConvert.DeserializeObject<Projects>(apiResponse);
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

        // POST: Admin/Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,APIEndPoint,UserName,Password")] Projects projects)
        {
            if (id != projects.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                    string endpoint = apiBaseUrl + "/Projects/" + id;
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(projects), Encoding.UTF8, "application/json");

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
                                //receivedProject = JsonConvert.DeserializeObject<Projects>(apiResponse);
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
                    if (!ProjectsExists(projects.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View(projects);
            }
            return View(projects);
        }

        // POST: Admin/Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            string endpoint = apiBaseUrl + "/Projects/";
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(endpoint + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (isAjax)
                                if(apiResponse == "")
                                    return Json(new { message = "done successfuly !", color = "success" });
                                else
                                    return Json(new { message = "Error ! Please try again .", color = "error" });
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                        return View("Index",new Projects());
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectsExists(string id)
        {
            Projects project = new Projects();
            string endpoint = apiBaseUrl + "/Projects/";

            using (var httpClient = new HttpClient())
            {
                using (var response = httpClient.GetAsync(endpoint + id))
                {
                    if (response.IsCompletedSuccessfully)
                    {
                        //string apiResponse =  response.Content.ReadAsStringAsync();
                        string apiResponse = response.Result.Content.ToString();
                        project = JsonConvert.DeserializeObject<Projects>(apiResponse);
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
