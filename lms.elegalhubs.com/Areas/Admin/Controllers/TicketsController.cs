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
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using library.elegalhubs.com.lms.Admin;
using Microsoft.AspNetCore.Identity;
using lms.elegalhubs.com.Areas.Admin.Models;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using lms.elegalhubs.com.Helpers;
using lms.elegalhubs.com.Controllers;
using lms.elegalhubs.com.Areas.Admin.Models.Tickets;

namespace lms.elegalhubs.com.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TicketsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;
        private readonly string projectID;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly IHubContext<MessageHub> _hubContext;

        public TicketsController(ApplicationDbContext context, IConfiguration configuration, 
            SignInManager<Users> signInManager, UserManager<Users> userManager, IHubContext<MessageHub> hubContext) :base(context,configuration,hubContext)
        {
            _context = context;
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
            projectID = _Configure.GetValue<string>("ProjectID");
            _signInManager = signInManager;
            _userManager = userManager;
            _hubContext = hubContext;

        }

        // GET: Admin/Tickets
        public async Task<IActionResult> Index()
        {
            List<IndexViewModel> reservationList = new List<IndexViewModel>();
            List<IndexViewModel> reservationList2 = new List<IndexViewModel>();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/Tickets";

                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string applicationDbContext1 = await Response.Content.ReadAsStringAsync();
                        reservationList = JsonConvert.DeserializeObject<List<IndexViewModel>>(applicationDbContext1);

                        foreach (var m in reservationList)
                        {
                            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result.Id;
                            var assignTo = _context.TicketEmployees.Where(p => p.TicketID == m.Id && p.IsCurrent).FirstOrDefault();
                            m.AssignTo = (assignTo == null ? "" : _context.Users.Where(p=>p.Id== assignTo.AssignTo).FirstOrDefaultAsync().Result.FullName);
                            if (!await _userManager.IsInRoleAsync(_userManager.GetUserAsync(User).Result, "SuperAdmin"))
                            {
                                if (assignTo != null)
                                {
                                    if (assignTo.AssignTo == currentUser)
                                        reservationList2.Add(m);
                                }
                            }

                        }

                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again or contact support department.");
                    }
                    if (await _userManager.IsInRoleAsync(_userManager.GetUserAsync(User).Result, "SuperAdmin"))
                        return View(reservationList);
                    else
                        return View(reservationList2);
                }
            }
        }

        // GET: Admin/Tickets/Details/5
        public async Task<IActionResult> Details(string id)
        {
            TicketIndexViewModel ticketDetailsList = new TicketIndexViewModel();
            if (id == null)
            {
                return NotFound();
            }

            Tickets tickets = new Tickets();
            List<TicketDetails> ticketDetails = new List<TicketDetails>();
            List<TicketAttachements> ticketAttachementsList = new List<TicketAttachements>();
            List<TicketComments> ticketCommentsList = new List<TicketComments>();

          

            string ticketendpoint = apiBaseUrl + "/Tickets/" + id;
            string ticketdetailsendpoint = apiBaseUrl + "/TicketDetails";
            string ticketattachendpoint = apiBaseUrl + "/TicketAttachements";
            string ticketcommentendpoint = apiBaseUrl + "/TicketComments";

            using (HttpClient client = new HttpClient())
            {
                using (var Response = await client.GetAsync(ticketendpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string applicationDbContext1 = await Response.Content.ReadAsStringAsync();
                        tickets = JsonConvert.DeserializeObject<Tickets>(applicationDbContext1);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again or contact support department.");
                    }
                }

                StringContent sTicketDetails = new StringContent(JsonConvert.SerializeObject(new TicketDetails
                {
                    TicketId = id
                }), Encoding.UTF8, "application/json");
                using (var Response = await client.SendAsync(new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(ticketdetailsendpoint + "/search"), Content = sTicketDetails }))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //return data 
                        string response = await Response.Content.ReadAsStringAsync();
                        ticketDetails = JsonConvert.DeserializeObject<List<TicketDetails>>(response);
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                        return View();
                    }
                }      

                StringContent sTicketAttachements = new StringContent(JsonConvert.SerializeObject(new TicketAttachements
                {
                    TicketId = id
                }), Encoding.UTF8, "application/json");
                using (var Response = await client.SendAsync(new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(ticketattachendpoint + "/search"), Content = sTicketAttachements }))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //return data 
                        string response = await Response.Content.ReadAsStringAsync();
                        ticketAttachementsList = JsonConvert.DeserializeObject<List<TicketAttachements>>(response);
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                        return View();
                    }
                }

                StringContent sTicketComments = new StringContent(JsonConvert.SerializeObject(new TicketComments
                {
                    TicketId = id
                }), Encoding.UTF8, "application/json");
                using (var Response = await client.SendAsync(new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(ticketcommentendpoint + "/search"), Content = sTicketComments }))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //return data 
                        string response = await Response.Content.ReadAsStringAsync();
                        ticketCommentsList = JsonConvert.DeserializeObject<List<TicketComments>>(response);
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                        return View();
                    }
                }
            }

            ticketDetailsList.Tickets = tickets;
            ticketDetailsList.TicketDetails = ticketDetails[0];
            ticketDetailsList.TicketAttachements = ticketAttachementsList.OrderBy(m => m.AddedDate);
            ticketDetailsList.TicketComments = ticketCommentsList.OrderBy( m => m.CommentDate);

            if (tickets == null)
            {
                return NotFound();
            }

            return View(ticketDetailsList);
        }

        // GET: Admin/Tickets/Create
        public async Task<IActionResult> Create()
        {
            List<CategoryTypes> CategoryTypesList = new List<CategoryTypes>();
            List<ContactMethods> ContactMethodsList = new List<ContactMethods>();
            List<ContactTypes> ContactTypesList = new List<ContactTypes>();
            List<Departments> DepartmentList = new List<Departments>();
            List<PriorityTypes> PriorityTypesList = new List<PriorityTypes>();
            List<StatusTypes> StatusTypesList = new List<StatusTypes>();
            List<TicketTypes> TicketTypesList = new List<TicketTypes>();
            List<Projects> ProjectsList = new List<Projects>();
            
            using (HttpClient client = new HttpClient())
            {
                string CategoryTypes = apiBaseUrl + "/CategoryTypes";
                string ContactMethods = apiBaseUrl + "/ContactMethods";
                string ContactTypes = apiBaseUrl + "/ContactTypes";
                string Department = apiBaseUrl + "/Departments";
                string PriorityTypes = apiBaseUrl + "/PriorityTypes";
                string StatusTypes = apiBaseUrl + "/StatusTypes";
                string TicketTypes = apiBaseUrl + "/TicketType";
                string Projects = apiBaseUrl + "/Projects";
                string apiresponse = "";

                using (var Response = await client.GetAsync(CategoryTypes))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        CategoryTypesList = JsonConvert.DeserializeObject<List<CategoryTypes>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(ContactMethods))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        ContactMethodsList = JsonConvert.DeserializeObject<List<ContactMethods>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(ContactTypes))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        ContactTypesList = JsonConvert.DeserializeObject<List<ContactTypes>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(Department))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        DepartmentList = JsonConvert.DeserializeObject<List<Departments>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }
                
                using (var Response = await client.GetAsync(PriorityTypes))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        PriorityTypesList = JsonConvert.DeserializeObject<List<PriorityTypes>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }
                
                using (var Response = await client.GetAsync(StatusTypes))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        StatusTypesList = JsonConvert.DeserializeObject<List<StatusTypes>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }
                
                using (var Response = await client.GetAsync(TicketTypes))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        TicketTypesList = JsonConvert.DeserializeObject<List<TicketTypes>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(Projects))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        ProjectsList = JsonConvert.DeserializeObject<List<Projects>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }
            }
            ViewData["CategoryId"] = new SelectList(CategoryTypesList,"Id", "Category");
            ViewData["PreferredContactMethodId"] = new SelectList(ContactMethodsList, "Id", "ContactMethod");
            ViewData["ContactId"] = new SelectList(ContactTypesList, "Id", "FirstName");
            ViewData["DepartmentId"] = new SelectList(DepartmentList, "Id", "Name");
            ViewData["PriorityId"] = new SelectList(PriorityTypesList, "Id", "Priority");
            ViewData["StatusId"] = new SelectList(StatusTypesList, "Id", "Status");
            ViewData["TicketTypeId"] = new SelectList(TicketTypesList, "Id", "TicketType");
            ViewData["ProjectId"] = new SelectList(ProjectsList, "Id", "Name");
            return View();
        }

        // POST: Admin/Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,TicketNumber,TicketTypeId,ContactId,Subject,StatusId,OpenDateTime,CloseDateTime,CategoryId,PriorityId,ProjectId,PreferredContactMethodId,UserId,DepartmentId")] Tickets tickets)
        public async Task<IActionResult> Create(TicketsDetailsViewModel ticketVM)
        {
            Tickets tickets = ticketVM.Tickets;
            TicketDetails ticketDetails = new TicketDetails
            {
                Description = ticketVM.TicketsDetails.Description,
                StartDateTime = ticketVM.TicketsDetails.StartDateTime,
                EndDateTime = ticketVM.TicketsDetails.EndDateTime,
            };
            TicketAttachements ticketAttach = new TicketAttachements();
 
            string ticketendpoint = apiBaseUrl + "/Tickets";
            string ticketdetailsendpoint = apiBaseUrl + "/TicketDetails";
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                var currentUser = _userManager.GetUserAsync(User).Result;
                var currentUserId = currentUser.Id;
                var currentUserUserName = currentUser.UserName;
                string userendpoint = apiBaseUrl + "/Users";
                string ticketattachmentendpoint = apiBaseUrl + "/TicketAttachements";

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
                    using (var response = await httpClient.PostAsync(apiBaseUrl + "/Users", user))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            User retuser = JsonConvert.DeserializeObject<User>(apiResponse);
                            tickets.UserId = retuser.Id;
                            //END check if the user exist in the ticket user or not -- if not then add it 

                            //START Add the ticket basic Info
                            StringContent ticket = new StringContent(JsonConvert.SerializeObject(tickets), Encoding.UTF8, "application/json");
                            Tickets retTicket = new Tickets();
                            using (var response1 = await httpClient.PostAsync(ticketendpoint, ticket))
                            {
                                if (response1.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    string apiResponse1 = await response1.Content.ReadAsStringAsync();
                                    retTicket = JsonConvert.DeserializeObject<Tickets>(apiResponse1);
                                    ticketDetails.TicketId = retTicket.Id;
                                    ticketAttach.TicketId = retTicket.Id;
                                    ticketAttach.UserId = retTicket.UserId;
                                    //END Add the ticket basic Info

                                    //START Add the ticket details Info
                                    StringContent ticketDet = new StringContent(JsonConvert.SerializeObject(ticketDetails), Encoding.UTF8, "application/json");
                                    TicketDetails returnTickDet = new TicketDetails();
                                    using (var response2 = await httpClient.PostAsync(ticketdetailsendpoint, ticketDet))
                                    {
                                        if (response2.StatusCode == System.Net.HttpStatusCode.OK)
                                        {
                                            string apiResponse2 = await response2.Content.ReadAsStringAsync();
                                            returnTickDet = JsonConvert.DeserializeObject<TicketDetails>(apiResponse2);
                                            //END Add the ticket basic Info

                                            //START Add the attachement details Info
                                            if (files.Count() > 0)
                                            {
                                                foreach (var file in files)
                                                {
                                                    TicketAttachements ticketAttach2 = new TicketAttachements();
                                                    ticketAttach2.TicketId = retTicket.Id;
                                                    ticketAttach2.UserId = retTicket.UserId;
                                                    string fileName = file.FileName;
                                                    string fileExt = file.ContentType;
                                                    ticketAttach2.FileName = fileName;
                                                    ticketAttach2.FileExt = fileExt;
                                                    using (var filestream = file.OpenReadStream())
                                                    {
                                                        using (var memorystream = new MemoryStream())
                                                        {
                                                            filestream.CopyTo(memorystream);
                                                            byte[] doc = memorystream.ToArray();
                                                            ticketAttach2.Attachment = doc;

                                                            StringContent ticketAttachement = new StringContent(JsonConvert.SerializeObject(ticketAttach2), Encoding.UTF8, "application/json");
                                                            TicketAttachements returnAttach = new TicketAttachements();
                                                            using (var response3 = await httpClient.PostAsync(ticketattachmentendpoint, ticketAttachement))
                                                            {
                                                                if (response3.StatusCode == System.Net.HttpStatusCode.OK)
                                                                {
                                                                    string apiResponse3 = await response3.Content.ReadAsStringAsync();
                                                                    returnAttach = JsonConvert.DeserializeObject<TicketAttachements>(apiResponse3);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            return RedirectToAction(nameof(Index));
                                            //END Add the ticket attachement Info
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            ModelState.Clear();
            ModelState.AddModelError(string.Empty, "Error ! Please try again .");

            //Get Data 
            List<CategoryTypes> CategoryTypesList = new List<CategoryTypes>();
            List<ContactMethods> ContactMethodsList = new List<ContactMethods>();
            List<ContactTypes> ContactTypesList = new List<ContactTypes>();
            List<Departments> DepartmentList = new List<Departments>();
            List<PriorityTypes> PriorityTypesList = new List<PriorityTypes>();
            List<StatusTypes> StatusTypesList = new List<StatusTypes>();
            List<TicketTypes> TicketTypesList = new List<TicketTypes>();
            List<Projects> ProjectsList = new List<Projects>();
            List<User> UserList = new List<User>();

            using (HttpClient client = new HttpClient())
            {
                string CategoryTypes = apiBaseUrl + "/CategoryTypes";
                string ContactMethods = apiBaseUrl + "/ContactMethods";
                string ContactTypes = apiBaseUrl + "/ContactTypes";
                string Department = apiBaseUrl + "/Departments";
                string PriorityTypes = apiBaseUrl + "/PriorityTypes";
                string StatusTypes = apiBaseUrl + "/StatusTypes";
                string TicketTypes = apiBaseUrl + "/TicketType";
                string Projects = apiBaseUrl + "/Projects";
                string User = apiBaseUrl + "/Users";
                string apiresponse = "";

                using (var Response = await client.GetAsync(CategoryTypes))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        CategoryTypesList = JsonConvert.DeserializeObject<List<CategoryTypes>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(ContactMethods))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        ContactMethodsList = JsonConvert.DeserializeObject<List<ContactMethods>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(ContactTypes))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        ContactTypesList = JsonConvert.DeserializeObject<List<ContactTypes>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(Department))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        DepartmentList = JsonConvert.DeserializeObject<List<Departments>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(PriorityTypes))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        PriorityTypesList = JsonConvert.DeserializeObject<List<PriorityTypes>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(StatusTypes))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        StatusTypesList = JsonConvert.DeserializeObject<List<StatusTypes>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(TicketTypes))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        TicketTypesList = JsonConvert.DeserializeObject<List<TicketTypes>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(User))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        UserList = JsonConvert.DeserializeObject<List<User>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }
                
                using (var Response = await client.GetAsync(Projects))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        ProjectsList = JsonConvert.DeserializeObject<List<Projects>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }
            }
            ViewData["CategoryId"] = new SelectList(CategoryTypesList, "Id", "Category");
            ViewData["PreferredContactMethodId"] = new SelectList(ContactMethodsList, "Id", "ContactMethod");
            ViewData["ContactId"] = new SelectList(ContactTypesList, "Id", "FirstName");
            ViewData["DepartmentId"] = new SelectList(DepartmentList, "Id", "Name");
            ViewData["PriorityId"] = new SelectList(PriorityTypesList, "Id", "Priority");
            ViewData["StatusId"] = new SelectList(StatusTypesList, "Id", "Status");
            ViewData["TicketTypeId"] = new SelectList(TicketTypesList, "Id", "TicketType");
            ViewData["ProjectId"] = new SelectList(ProjectsList, "Id", "Name");
            ViewData["UserId"] = new SelectList(UserList, "Id", "UserName");
            return View(ticketVM);
        }
 
        [HttpGet]
        public async Task<FileResult> DocumentDownload(string documentId)
        {
            TicketAttachements document = new TicketAttachements();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/TicketAttachements/" + documentId;

                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string applicationDbContext1 = await Response.Content.ReadAsStringAsync();
                        document = JsonConvert.DeserializeObject<TicketAttachements>(applicationDbContext1);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                    }
                }
            }





            //var document = BusinessLayer.GetDocumentsByDocument(documentId, AuthenticationHandler.HostProtocol).FirstOrDefault();

            //return File(document.FileBytes, document.FileType, document.FileName);

            return  File(document.Attachment, document.FileExt, document.FileName);

        }

        // GET: Admin/Tickets/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            TicketIndexViewModel ticketDetailsList = new TicketIndexViewModel();
            if (id == null)
            {
                return NotFound();
            }

            Tickets tickets = new Tickets();
            List<TicketDetails> ticketDetails = new List<TicketDetails>();
            //List<TicketAttachements> ticketAttachementsList = new List<TicketAttachements>();
            //List<TicketComments> ticketCommentsList = new List<TicketComments>();



            string ticketendpoint = apiBaseUrl + "/Tickets/" + id;
            string ticketdetailsendpoint = apiBaseUrl + "/TicketDetails";
            //string ticketattachendpoint = apiBaseUrl + "/TicketAttachements";
            //string ticketcommentendpoint = apiBaseUrl + "/TicketComments";

            using (HttpClient client = new HttpClient())
            {
                using (var Response = await client.GetAsync(ticketendpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string applicationDbContext1 = await Response.Content.ReadAsStringAsync();
                        tickets = JsonConvert.DeserializeObject<Tickets>(applicationDbContext1);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again or contact support department.");
                    }
                }

                StringContent sTicketDetails = new StringContent(JsonConvert.SerializeObject(new TicketDetails
                {
                    TicketId = id
                }), Encoding.UTF8, "application/json");
                using (var Response = await client.SendAsync(new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(ticketdetailsendpoint + "/search"), Content = sTicketDetails }))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //return data 
                        string response = await Response.Content.ReadAsStringAsync();
                        ticketDetails = JsonConvert.DeserializeObject<List<TicketDetails>>(response);
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {

                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                        return View();
                    }
                }
        
            }

            ticketDetailsList.Tickets = tickets;
            ticketDetailsList.TicketDetails = ticketDetails[0];

            if (tickets == null)
            {
                return NotFound();
            }


            //Get Data 
            List<CategoryTypes> CategoryTypesList = new List<CategoryTypes>();
            List<ContactMethods> ContactMethodsList = new List<ContactMethods>();
            List<ContactTypes> ContactTypesList = new List<ContactTypes>();
            List<Departments> DepartmentList = new List<Departments>();
            List<PriorityTypes> PriorityTypesList = new List<PriorityTypes>();
            List<StatusTypes> StatusTypesList = new List<StatusTypes>();
            List<TicketTypes> TicketTypesList = new List<TicketTypes>();
            List<Projects> ProjectsList = new List<Projects>();
            List<User> UserList = new List<User>();

            using (HttpClient client = new HttpClient())
            {
                string CategoryTypes = apiBaseUrl + "/CategoryTypes";
                string ContactMethods = apiBaseUrl + "/ContactMethods";
                string ContactTypes = apiBaseUrl + "/ContactTypes";
                string Department = apiBaseUrl + "/Departments";
                string PriorityTypes = apiBaseUrl + "/PriorityTypes";
                string StatusTypes = apiBaseUrl + "/StatusTypes";
                string TicketTypes = apiBaseUrl + "/TicketType";
                string Projects = apiBaseUrl + "/Projects";
                string User = apiBaseUrl + "/Users";
                string apiresponse = "";

                using (var Response = await client.GetAsync(CategoryTypes))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        CategoryTypesList = JsonConvert.DeserializeObject<List<CategoryTypes>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(ContactMethods))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        ContactMethodsList = JsonConvert.DeserializeObject<List<ContactMethods>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(ContactTypes))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        ContactTypesList = JsonConvert.DeserializeObject<List<ContactTypes>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(Department))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        DepartmentList = JsonConvert.DeserializeObject<List<Departments>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(PriorityTypes))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        PriorityTypesList = JsonConvert.DeserializeObject<List<PriorityTypes>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(StatusTypes))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        StatusTypesList = JsonConvert.DeserializeObject<List<StatusTypes>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(TicketTypes))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        TicketTypesList = JsonConvert.DeserializeObject<List<TicketTypes>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(User))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        UserList = JsonConvert.DeserializeObject<List<User>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }

                using (var Response = await client.GetAsync(Projects))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiresponse = await Response.Content.ReadAsStringAsync();
                        ProjectsList = JsonConvert.DeserializeObject<List<Projects>>(apiresponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                    }
                }
            }
            ViewData["CategoryId"] = new SelectList(CategoryTypesList, "Id", "Category");
            ViewData["PreferredContactMethodId"] = new SelectList(ContactMethodsList, "Id", "ContactMethod");
            ViewData["ContactId"] = new SelectList(ContactTypesList, "Id", "FirstName");
            ViewData["DepartmentId"] = new SelectList(DepartmentList, "Id", "Name");
            ViewData["PriorityId"] = new SelectList(PriorityTypesList, "Id", "Priority");
            ViewData["StatusId"] = new SelectList(StatusTypesList, "Id", "Status");
            ViewData["TicketTypeId"] = new SelectList(TicketTypesList, "Id", "TicketType");
            ViewData["ProjectId"] = new SelectList(ProjectsList, "Id", "Name");
            ViewData["UserId"] = new SelectList(UserList, "Id", "UserName");
            ViewData["AssignTo"] = new SelectList(_context.Users.ToList(), "Id", "FullName");

            return View(ticketDetailsList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, IndexViewModel ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            try
            {
                bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                string endpoint = apiBaseUrl + "/Tickets/Edit/" + id;
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(ticket), Encoding.UTF8, "application/json");
                    Task<Tickets> ticketPrev = TicketInfo(id);
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
                            //return RedirectToAction(nameof(Index));
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
                if (!TicketsExists(ticket.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("TicketsChanges");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignTicket(string id, string AssignTo)
        {
            
            try
            {

               var  isExist =  _context.TicketEmployees.Where(p => p.AssignTo == AssignTo && p.TicketID == id &&p.IsCurrent == true).Count() > 0;
                if (!isExist)
                {
                    var lastAssign = _context.TicketEmployees.Where(p => p.TicketID == id && p.IsCurrent == true).FirstOrDefault();
                    if(lastAssign != null)
                    {
                        lastAssign.IsCurrent = false;
                        await _context.SaveChangesAsync();
                    }

                    TicketEmployees te = new TicketEmployees();
                    te.TicketID = id;
                    te.AssignTo = AssignTo;
                    _context.Add(te);
                    await _context.SaveChangesAsync();
                    var currentUserId = _userManager.GetUserAsync(User).Result.Id;
                    Notifications notfiy = new Notifications();
                    notfiy.Subject = "New ticket";
                    notfiy.Message = "New ticket assign to you";
                    notfiy.UserTo = AssignTo;
                    notfiy.NotificationTypeId = "1";
                    notfiy.UserFrom = currentUserId;
                    await SendNotification(notfiy);
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError(string.Empty, "This Ticket Aleardy assigned to this user");
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                 
            }
            return RedirectToAction("TicketsChanges");
        }

        private bool TicketsExists(string id)
        {
            Tickets ticket = new Tickets();
            string endpoint = apiBaseUrl + "/Tickets/";

            using (var httpClient = new HttpClient())
            {
                using (var response = httpClient.GetAsync(endpoint + id))
                {
                    if (response.IsCompletedSuccessfully)
                    {
                        //string apiResponse =  response.Content.ReadAsStringAsync();
                        string apiResponse = response.Result.Content.ToString();
                        ticket = JsonConvert.DeserializeObject<Tickets>(apiResponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                    }
                }
            }

            if (ticket == null)
            {
                return false;
            }

            return ticket.Id == null ? false : true;
        }
        public async Task<IActionResult> TicketsChanges()
        {
            List<IndexViewModel> reservationList = new List<IndexViewModel>();
            List<IndexViewModel> reservationList2 = new List<IndexViewModel>();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/Tickets";

                using (var Response1 = await client.GetAsync(endpoint))
                {
                    if (Response1.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string applicationDbContext1 = await Response1.Content.ReadAsStringAsync();
                        reservationList = JsonConvert.DeserializeObject<List<IndexViewModel>>(applicationDbContext1);
                        foreach(var m in reservationList)
                        {
                            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result.Id;
                            var assignTo = _context.TicketEmployees.Where(p => p.TicketID == m.Id && p.IsCurrent).FirstOrDefault();
                            m.AssignTo = (assignTo == null ? "" : assignTo.AssignTo);
                            if (!await _userManager.IsInRoleAsync(_userManager.GetUserAsync(User).Result, "SuperAdmin"))
                            {
                                if (assignTo != null)
                                {
                                    if (assignTo.AssignTo == currentUser)
                                        reservationList2.Add(m);
                                }
                            }
                            
                        }
                        //Get Data 
                         List<PriorityTypes> PriorityTypesList = new List<PriorityTypes>();
                        List<StatusTypes> StatusTypesList = new List<StatusTypes>();
                        //var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                        //List<Users> UserList = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();
                        List<Users> UserList = _context.Users.Where(p=>p.IsDelete == false).ToList();
                        string PriorityTypes = apiBaseUrl + "/PriorityTypes";
                        string StatusTypes = apiBaseUrl + "/StatusTypes";
                        //string User = apiBaseUrl + "/Users";
                        string apiresponse = "";

                        using (var Response = await client.GetAsync(PriorityTypes))
                        {
                            if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                apiresponse = await Response.Content.ReadAsStringAsync();
                                PriorityTypesList = JsonConvert.DeserializeObject<List<PriorityTypes>>(apiresponse);
                            }
                            else
                            {
                                ModelState.Clear();
                                ModelState.AddModelError(string.Empty, "Error ! please try again late");
                            }
                        }
                        using (var Response = await client.GetAsync(StatusTypes))
                        {
                            if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                apiresponse = await Response.Content.ReadAsStringAsync();
                                StatusTypesList = JsonConvert.DeserializeObject<List<StatusTypes>>(apiresponse);
                            }
                            else
                            {
                                ModelState.Clear();
                                ModelState.AddModelError(string.Empty, "Error ! please try again late");
                            }
                        }
                        //using (var Response = await client.GetAsync(User))
                        //{
                        //    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        //    {
                        //        apiresponse = await Response.Content.ReadAsStringAsync();
                        //        UserList = JsonConvert.DeserializeObject<List<User>>(apiresponse);
                        //    }
                        //    else
                        //    {
                        //        ModelState.Clear();
                        //        ModelState.AddModelError(string.Empty, "Error ! please try again late");
                        //    }
                        //}
                        ViewData["PriorityId"] = new SelectList(PriorityTypesList, "Id", "Priority");
                        ViewData["StatusId"] = new SelectList(StatusTypesList, "Id", "Status");
                        ViewData["AssignTo"] = new SelectList(UserList, "Id", "FullName");
                        
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again or contact support department.");
                    }
                    if (await _userManager.IsInRoleAsync(_userManager.GetUserAsync(User).Result, "SuperAdmin")){
                        return View(reservationList);
                    }
                    else{
                        return View(reservationList2);
                    }
                }
            }
        }

        private async Task<Tickets> TicketInfo(string id)
        {
            Tickets ticket = new Tickets();
            string endpoint = apiBaseUrl + "/Tickets/";

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(endpoint + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //string apiResponse =  response.Content.ReadAsStringAsync();
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        //string apiResponse = response.Content.ToString();
                        ticket = JsonConvert.DeserializeObject<Tickets>(apiResponse);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                    }
                }
            }
            return ticket;
        }

        // POST: Admin/Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Id,TicketNumber,TicketTypeId,ContactId,Subject,StatusId,OpenDateTime,CloseDateTime,CategoryId,PriorityId,PreferredContactMethodId,UserId,DepartmentId")] Tickets tickets)
        //{
        //    if (id != tickets.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(tickets);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TicketsExists(tickets.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Set<CategoryTypes>(), "Id", "Id", tickets.CategoryId);
        //    ViewData["PreferredContactMethodId"] = new SelectList(_context.Set<ContactMethods>(), "Id", "Id", tickets.PreferredContactMethodId);
        //    ViewData["ContactId"] = new SelectList(_context.Set<ContactTypes>(), "Id", "Id", tickets.ContactId);
        //    ViewData["DepartmentId"] = new SelectList(_context.Set<Departments>(), "Id", "Id", tickets.DepartmentId);
        //    ViewData["PriorityId"] = new SelectList(_context.Set<PriorityTypes>(), "Id", "Id", tickets.PriorityId);
        //    ViewData["StatusId"] = new SelectList(_context.Set<StatusTypes>(), "Id", "Id", tickets.StatusId);
        //    ViewData["TicketTypeId"] = new SelectList(_context.Set<TicketTypes>(), "Id", "Id", tickets.TicketTypeId);
        //    ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", tickets.UserId);
        //    return View(tickets);
        //}

        // GET: Admin/Tickets/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tickets = await _context.Tickets
        //        .Include(t => t.CategoryTypes)
        //        //.Include(t => t.ContactMethods)
        //        .Include(t => t.ContactTypes)
        //        .Include(t => t.Department)
        //        .Include(t => t.PriorityTypes)
        //        .Include(t => t.StatusTypes)
        //        .Include(t => t.TicketTypes)
        //        .Include(t => t.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (tickets == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(tickets);
        //}

        //// POST: Admin/Tickets/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var tickets = await _context.Tickets.FindAsync(id);
        //    _context.Tickets.Remove(tickets);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool TicketsExists(string id)
        //{
        //    return _context.Tickets.Any(e => e.Id == id);
        //}

        // POST: Admin/Projects/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            string endpoint = apiBaseUrl + "/Tickets/";
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(endpoint + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (isAjax)
                            if (apiResponse == "")
                            {
                                var em = _context.TicketEmployees.Where(p=>p.TicketID==id).ToList();
                                _context.TicketEmployees.RemoveRange(em);
                                _context.SaveChanges();
                                return Json(new { message = "done successfuly !", color = "success" });
                            }
                            else
                                return Json(new { message = "Error ! Please try again .", color = "error" });
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Error ! Please try again .");
                        return View("Index", new Projects());
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
