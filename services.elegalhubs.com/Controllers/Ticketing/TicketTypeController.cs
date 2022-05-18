using library.elegalhubs.com.Ticketing;
using Microsoft.AspNetCore.Mvc;
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
    public class TicketTypeController : ControllerBase
    {

        private readonly ICommanRepository<TicketTypes> _ticketTypeR;

        public TicketTypeController(ICommanRepository<TicketTypes> context)
        {
            _ticketTypeR = context;
          
        }

        // GET: api/<TicketTypeController>
        [HttpGet]
        public async Task<IEnumerable<TicketTypes>> Get()
        {
            return await _ticketTypeR.Index();
        }

        // GET api/<TicketTypeController>/5
        [HttpGet("{id}")]
        public async Task<TicketTypes> Get(string id)
        {
            return await _ticketTypeR.Details(id);
        }

        // POST api/<TicketTypeController>
        [HttpPost]
        public async Task<TicketTypes> Post([FromBody] TicketTypes value)
        {
            return await _ticketTypeR.Create(value);
        }

        // PUT api/<TicketTypeController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] TicketTypes value)
        {
             await _ticketTypeR.Edit(value);
        }

        // DELETE api/<TicketTypeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _ticketTypeR.DeleteConfirmed(id);
        }
    }
}
