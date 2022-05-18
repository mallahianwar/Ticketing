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
    [Area("Admin")]
    [Authorize]
    public class ContactTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;
        public ContactTypesController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }


        // GET: Admin/ContactTypes
        public async Task<IActionResult> Index()
        {
            List<ContactTypes> ContactTypesList = new List<ContactTypes>();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/ContactTypes";

                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string applicationDbContext1 = await Response.Content.ReadAsStringAsync();
                        ContactTypesList = JsonConvert.DeserializeObject<List<ContactTypes>>(applicationDbContext1);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                    }
                }
            }
            return View(ContactTypesList);
        }

        //GET: Admin/ContactTypes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var ContactTypes = await _context.ContactTypes
            //    .FirstOrDefaultAsync(m => m.Id == id);



            ContactTypes ContactTypes = new ContactTypes();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/ContactTypes/" + id;

                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string applicationDbContext1 = await Response.Content.ReadAsStringAsync();
                        ContactTypes = JsonConvert.DeserializeObject<ContactTypes>(applicationDbContext1);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                    }
                }
            }


            if (ContactTypes == null)
            {
                return NotFound();
            }

            return View(ContactTypes);
        }

        // GET: Admin/ContactTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ContactTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,ContactNumber")] ContactTypes ContactTypes)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            ContactTypes receivedcontacttype = new ContactTypes();
            string endpoint = apiBaseUrl + "/ContactTypes";
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(ContactTypes), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(endpoint, content))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            receivedcontacttype = JsonConvert.DeserializeObject<ContactTypes>(apiResponse);
                            if (isAjax)
                                if (string.IsNullOrEmpty(receivedcontacttype.Id))
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
            return View(receivedcontacttype);
        }

        // GET: Admin/ContactTypes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContactTypes contacttype = new ContactTypes();
            string endpoint = apiBaseUrl + "/ContactTypes/";

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(endpoint + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        contacttype = JsonConvert.DeserializeObject<ContactTypes>(apiResponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                    }

                }
            }

            if (contacttype == null)
            {
                return NotFound();
            }
            return View(contacttype);
        }

        // POST: Admin/ContactTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,Email,ContactNumber")] ContactTypes ContactTypes)
        {
            if (id != ContactTypes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                    string endpoint = apiBaseUrl + "/ContactTypes/" + id;
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(ContactTypes), Encoding.UTF8, "application/json");

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
                                //receivedcontacttype = JsonConvert.DeserializeObject<ContactTypes>(apiResponse);
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
                    if (!ContactTypesExists(ContactTypes.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View(ContactTypes);
            }
            return View(ContactTypes);
        }

        // POST: Admin/ContactTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            string endpoint = apiBaseUrl + "/ContactTypes/";
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
                        return View("Index", new ContactTypes());
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ContactTypesExists(string id)
        {
            ContactTypes contacttype = new ContactTypes();
            string endpoint = apiBaseUrl + "/ContactTypes/";

            using (var httpClient = new HttpClient())
            {
                using (var response = httpClient.GetAsync(endpoint + id))
                {
                    if (response.IsCompletedSuccessfully)
                    {
                        //string apiResponse =  response.Content.ReadAsStringAsync();
                        string apiResponse = response.Result.Content.ToString();
                        contacttype = JsonConvert.DeserializeObject<ContactTypes>(apiResponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                    }
                }
            }

            if (contacttype == null)
            {
                return false;
            }

            return contacttype.Id == null ? false : true;
        }
    }
}
