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
    public class TicketAttachementsController : ControllerBase
    {

        private readonly ICommanRepository<TicketAttachements> _ticketAttachementR;
        private readonly TicketAttachementsRepository _ticketAttach;

        public TicketAttachementsController(ICommanRepository<TicketAttachements> context,
            TicketAttachementsRepository attach)
        {
            _ticketAttachementR = context;
            _ticketAttach = attach;
          
        }

        // GET: api/<TicketTypeController>
        [HttpGet]
        public async Task<IEnumerable<TicketAttachements>> Get()
        {
            return await _ticketAttachementR.Index();
        }

        // GET api/<TicketTypeController>/5
        [HttpGet("{id}")]
        public async Task<TicketAttachements> Get(string id)
        {
            return await _ticketAttachementR.Details(id);
        }

        [HttpGet("search")]
        public async Task<IEnumerable<TicketAttachements>> Get([FromBody] TicketVM value)
        {
            return await _ticketAttach.SearchVM(value);
        }
        
        // POST api/<TicketTypeController>
        [HttpPost]
        public async Task<TicketAttachements> Post([FromBody] TicketAttachements value)
        {
            return await _ticketAttachementR.Create(value);
        }

        // PUT api/<TicketTypeController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] TicketAttachements value)
        {
             await _ticketAttachementR.Edit(value);
        }

        // DELETE api/<TicketTypeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _ticketAttachementR.DeleteConfirmed(id);
        }
    }
}
