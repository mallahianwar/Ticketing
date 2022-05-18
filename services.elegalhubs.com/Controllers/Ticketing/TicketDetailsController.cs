using library.elegalhubs.com.Ticketing;
using Microsoft.AspNetCore.Mvc;
using services.elegalhubs.com.Model;
using services.elegalhubs.com.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace services.elegalhubs.com.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketDetailsController : ControllerBase
    {

        private readonly ICommanRepository<TicketDetails> _ticketDetailR;
        private readonly TicketDetailsRepository _ticketDetail;

        public TicketDetailsController(ICommanRepository<TicketDetails> context,
            TicketDetailsRepository ticketDetail)
        {
            _ticketDetailR = context;
            _ticketDetail = ticketDetail;
          
        }

        // GET: api/<TicketTypeController>
        [HttpGet]
        public async Task<IEnumerable<TicketDetails>> Get()
        {
            return await _ticketDetailR.Index();
        }

        // GET api/<TicketTypeController>/5
        [HttpGet("{id}")]
        public async Task<TicketDetails> Get(string id)
        {
            return await _ticketDetailR.Details(id);
        }

        [HttpGet("search")]
        public async Task<IEnumerable<TicketDetails>> Get([FromBody] TicketVM value)
        {
            return await _ticketDetail.SearchVM(value); ;
        }

        // POST api/<TicketTypeController>
        [HttpPost]
        public async Task<TicketDetails> Post([FromBody] TicketDetails value)
        {
            return await _ticketDetailR.Create(value);
        }

        // PUT api/<TicketTypeController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] TicketDetails value)
        {
             await _ticketDetailR.Edit(value);
        }

        // DELETE api/<TicketTypeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _ticketDetailR.DeleteConfirmed(id);
        }
    }
}
