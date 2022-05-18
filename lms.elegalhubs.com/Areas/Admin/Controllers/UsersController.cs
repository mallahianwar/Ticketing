using library.elegalhubs.com.lms.Admin;
using library.elegalhubs.com.Ticketing;
using lms.elegalhubs.com.Areas.Admin.Models;
using lms.elegalhubs.com.Data;
using lms.elegalhubs.com.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lms.elegalhubs.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly string apiBaseUrl;
        private readonly string projectID;
        private readonly IConfiguration _Configure;

        public UsersController(UserManager<Users> userManager, SignInManager<Users> signInManager, ApplicationDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
            projectID = _Configure.GetValue<string>("ProjectID");
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();

            var userRolesViewModel = new List<UserRolesIndexViewModel>();
            foreach (Users user in allUsersExceptCurrentUser)
            {
                var thisViewModel = new UserRolesIndexViewModel();
                thisViewModel.Id = user.Id;
                thisViewModel.FullName = user.FullName;
                thisViewModel.Email = user.Email;
                thisViewModel.PhoneNumber = user.PhoneNumber;
                thisViewModel.Avtar = user.Avtar;
                thisViewModel.CreatedAt = user.CreatedAt;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }

            return View(userRolesViewModel);
        }
        private async Task<List<string>> GetUserRoles(Users user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
        public async Task<IActionResult> List()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();
            return View(allUsersExceptCurrentUser);
        }
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Users user,string img)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(img))
                    {
                        string image = img.Replace(@"""", string.Empty);
                        byte[] bytes = Convert.FromBase64String(image);
                        user.Avtar = bytes;
                    }

                    var usr = new Users
                    {
                        UserName = user.Email,
                        Email = user.Email,
                        FullName = user.FullName,
                        PhoneNumber = user.PhoneNumber,
                        Avtar = user.Avtar,
                        Address = user.Address,
                        EmailConfirmed = true
                    };
                    var result = await _userManager.CreateAsync(usr, user.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(usr, Roles.Employee.ToString());

                        string userendpoint = apiBaseUrl + "/Users";
                        using (var httpClient = new HttpClient())
                        {

                            StringContent user_send = new StringContent(JsonConvert.SerializeObject(new User
                            {
                                UserId = usr.Id,
                                ProjectId = projectID,//admin
                                UserName = usr.UserName,
                                MobileNumber = usr.PhoneNumber,
                                EmailAddress = usr.Email,
                                Name = usr.FullName

                            }), Encoding.UTF8, "application/json");
                            await httpClient.PostAsync(userendpoint, user_send);
                        }
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = user.Email });
                        }
                    }
                    if (result.Errors.Any())
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(user);
                    }
                    if (isAjax)
                        return Json(new { message = "done successfuly !", color = "success" });
                    else
                        return View("Index");

                        
                }
                return View(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(user);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Users user,string img)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (ModelState.IsValid)
            {
                try
                {
                    Users usr = await _userManager.FindByIdAsync(id);
                    if(usr != null)
                    {
                        if (!string.IsNullOrEmpty(img))
                        {
                            string image = img.Replace(@"""", string.Empty);
                            byte[] bytes = Convert.FromBase64String(image);
                            usr.Avtar = bytes;
                        }
                        if (!string.IsNullOrEmpty(user.UserName))
                            usr.UserName = user.UserName;
                        
                        if (!string.IsNullOrEmpty(user.Email))
                            usr.Email = user.Email;
                        
                        if (!string.IsNullOrEmpty(user.FullName))
                            usr.FullName = user.FullName;
                        
                        if (!string.IsNullOrEmpty(user.PhoneNumber))
                            usr.PhoneNumber = user.PhoneNumber;

                        if (!string.IsNullOrEmpty(user.Address))
                            usr.Address = user.Address;

                        var result = await _userManager.UpdateAsync(usr);
                        if (result.Succeeded)
                        {
                            using (var httpClient = new HttpClient())
                            { 
                                StringContent user1 = new StringContent(JsonConvert.SerializeObject(new User
                                {
                                    UserId = usr.Id,
                                    ProjectId = projectID,//admin
                                    UserName = usr.UserName,
                                    MobileNumber = usr.PhoneNumber,
                                    EmailAddress = usr.Email,
                                    Name = usr.FullName

                                }), Encoding.UTF8, "application/json");
                                
                                using (var response = await httpClient.PostAsync(apiBaseUrl + "/Users", user1))
                                {
                                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        string apiResponse = await response.Content.ReadAsStringAsync();
                                        User retuser = JsonConvert.DeserializeObject<User>(apiResponse);

                                        StringContent user2 = new StringContent(JsonConvert.SerializeObject(new User
                                        {
                                            UserId = usr.Id,
                                            ProjectId = projectID,//admin
                                            UserName = usr.UserName,
                                            MobileNumber = usr.PhoneNumber,
                                            EmailAddress = usr.Email,
                                            Name = usr.FullName,
                                            Id = retuser.Id
                                        }), Encoding.UTF8, "application/json");

                                        using (var response2 = await httpClient.PutAsync(apiBaseUrl + "/Users/" + retuser.Id, user2))
                                        {
                                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                             
                            if (isAjax)
                                return Json(new { message = "done successfuly !", color = "success" });
                            else
                                return View("Index");
                        }
                        if (result.Errors.Any())
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            return View(user);
                        }
                        return Json(new { message = "done successfuly !" });
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
            }
            return View(user);

        }

        // POST: Admin/Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(string id)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (id == null)
            {
                return Json(new { message = "error , try again !", color = "error" });
            }
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return Json(new { message = "user not exist !", color = "error" });
            }
            try
            {
                _context.Users.Remove(user);                
                await _context.SaveChangesAsync();
                string endpoint = apiBaseUrl + "/Users/";
                using (var client = new HttpClient())
                {
                    List<User> getUser = new List<User>();
                    StringContent sUsers = new StringContent(JsonConvert.SerializeObject(new User
                    {
                        ProjectId = projectID,
                        UserId = user.Id
                    }), Encoding.UTF8, "application/json"); ;
                    using (var Response = await client.SendAsync(new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(endpoint + "search"), Content = sUsers }))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            //return data 
                            string response = await Response.Content.ReadAsStringAsync();
                            getUser = JsonConvert.DeserializeObject<List<User>>(response);
                            if (getUser.Count > 0)
                                await client.DeleteAsync(endpoint + getUser[0].Id);
                        }
                    }

                }
            }
            catch
            {
                return Json(new { message = "error !", color = "error" });

            }
            return Json(new { message = "done successfuly !", color = "success" });
        }
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
