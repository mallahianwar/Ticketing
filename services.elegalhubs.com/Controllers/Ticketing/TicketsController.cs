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
    public class TicketsController : ControllerBase
    {

        private readonly ICommanRepository<Tickets> _ticketR;
        private readonly TicketsRepository _ticket;

        public TicketsController(ICommanRepository<Tickets> context, TicketsRepository _context)
        {
            _ticketR = context;
            _ticket  = _context;       
        }

        // GET: api/<TicketTypeController>
        [HttpGet]
        public async Task<IEnumerable<Tickets>> Get()
        {
            return await _ticketR.Index();
        }

        // GET api/<TicketTypeController>/5
        [HttpGet("{id}")]
        public async Task<Tickets> Get(string id)
        {
            return await _ticketR.Details(id);
        }

        // POST api/<TicketTypeController>
        [HttpPost]
        public async Task<Tickets> Post([FromBody] Tickets value)
        {
            return await _ticketR.Create(value);
        }

        // PUT api/<TicketTypeController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Tickets value)
        {
             await _ticketR.Edit(value);
        }
        [HttpPut("Edit/{id}")]
        public async Task Put(string id, [FromBody] EditTicketVM value)
        {
             await _ticket.Edit(value);
        }
 

        // DELETE api/<TicketTypeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _ticketR.DeleteConfirmed(id);
        }
    }
}
