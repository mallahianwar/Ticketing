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
using Microsoft.AspNetCore.Identity;
using library.elegalhubs.com.lms.Admin;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace lms.elegalhubs.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TicketCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;
        private readonly string projectID;
        private readonly UserManager<Users> _userManager;
        public TicketCommentsController(ApplicationDbContext context, IConfiguration configuration,UserManager<Users> userManager)
        {
            _context = context;
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
            _userManager = userManager;
            projectID = _Configure.GetValue<string>("ProjectID");

        }


        // POST: Admin/TicketComments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TicketId,Description,UserId,CommentDate")] TicketComments ticketComments)
        {
            ticketComments.CommentDate = DateTime.Now;
            TicketComments receivedTicketComments = new TicketComments();
            string endpoint = apiBaseUrl + "/TicketComments";

            var currentUser = _userManager.GetUserAsync(User).Result;
            var currentUserId = currentUser.Id;
            var currentUserUserName = currentUser.UserName;
            string userendpoint = apiBaseUrl + "/Users";
            if (ModelState.IsValid)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        //START check if the user exist in the ticket user or not -- if not then add it 
                        StringContent user = new StringContent(JsonConvert.SerializeObject(new User
                        {
                            UserId = currentUserId,
                            ProjectId = projectID,//admin
                            UserName = currentUserUserName,
                            MobileNumber = currentUser.PhoneNumber,
                            EmailAddress = currentUser.Email,
                            Name = currentUser.FullName

                        }), Encoding.UTF8, "application/json");
                        //not found in tickiting user then we need to add it
                        using (var response = await httpClient.PostAsync(apiBaseUrl + "/Users", user))
                        {
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                User retuser = JsonConvert.DeserializeObject<User>(apiResponse);
                                ticketComments.UserId = retuser.Id;
                                //END check if the user exist in the ticket user or not -- if not then add it 

                                StringContent content = new StringContent(JsonConvert.SerializeObject(ticketComments), Encoding.UTF8, "application/json");
                                using (var response1 = await httpClient.PostAsync(endpoint, content))
                                {
                                    if (response1.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        string apiResponse1 = await response1.Content.ReadAsStringAsync();
                                        receivedTicketComments = JsonConvert.DeserializeObject<TicketComments>(apiResponse1);
                                    }
                                }
                            }
                        }

                        
                    }
                }
                catch (Exception ex)
                {
                    //return Json(new { message = "Error ! Please try again .", color = "error" });
                }
            }
            return RedirectToAction( "Details", "Tickets", new { Id=ticketComments.TicketId });
        }


        // POST: Admin/TicketComments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var ticketComments = await _context.TicketComments.FindAsync(id);
        //    _context.TicketComments.Remove(ticketComments);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
