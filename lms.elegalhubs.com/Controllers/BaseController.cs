using library.elegalhubs.com.Ticketing;
using lms.elegalhubs.com.Areas.Admin.Models;
using lms.elegalhubs.com.Data;
using lms.elegalhubs.com.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace lms.elegalhubs.com.Controllers
{
    public class BaseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;
        private readonly IHubContext<MessageHub> _hubContext;

        public BaseController(ApplicationDbContext context, IConfiguration configuration, IHubContext<MessageHub> hubContext)
        {
            _context = context;
            _Configure = configuration;
            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
            _hubContext = hubContext;
        }
        public async Task<bool> SendNotification(Notifications notify)
        {
            try
            {
                _context.Add(notify);
                await _context.SaveChangesAsync();
                if(notify.NotificationTypeId == "1"){
                    if (string.IsNullOrEmpty(notify.UserTo))
                        await _hubContext.Clients.All.SendAsync("ReceiveMessageHandler", notify.Message);
                    else
                        await _hubContext.Clients.User(notify.UserTo).SendAsync("ReceiveMessageHandler", notify.Message);
                }else if(notify.NotificationTypeId == "2"){
                    var result = MailHelper.SendEmail(notify.Subject, notify.Reciver.Email, notify.Message);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }        
        public async Task<TicketIndexViewModel> TicketInfo(string id,bool IsDetail = false)
        {
            TicketIndexViewModel ticketDetailsList = new TicketIndexViewModel();
            try
            {
                Tickets tickets = new Tickets();
                List<TicketDetails> ticketDetails = new List<TicketDetails>();
                string ticketendpoint = apiBaseUrl + "/Tickets/" + id;
                string ticketdetailsendpoint = apiBaseUrl + "/TicketDetails";

                using (HttpClient client = new HttpClient())
                {
                    using (var Response = await client.GetAsync(ticketendpoint))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string applicationDbContext1 = await Response.Content.ReadAsStringAsync();
                            tickets = JsonConvert.DeserializeObject<Tickets>(applicationDbContext1);
                            ticketDetailsList.Tickets = tickets;
                        }
                    }
                    if (IsDetail)
                    {
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
                                ticketDetailsList.TicketDetails = ticketDetails[0];
                            }
                        }
                    }
                                        
                }
            }
            catch (Exception ex)
            {
                 
            }
            return ticketDetailsList;
        }
    }
}
