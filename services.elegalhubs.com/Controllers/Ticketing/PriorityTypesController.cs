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
    public class PriorityTypesController : ControllerBase
    {

        private readonly ICommanRepository<PriorityTypes> _priorityTypeR;

        public PriorityTypesController(ICommanRepository<PriorityTypes> context)
        {
            _priorityTypeR = context;
          
        }

        // GET: api/<TicketTypeController>
        //[HttpGet]
        public async Task<IEnumerable<PriorityTypes>> Get()
        {
            return await _priorityTypeR.Index();
        }

        // GET api/<TicketTypeController>/5
        [HttpGet("{id}")]
        public async Task<PriorityTypes> Get(string id)
        {
            return await _priorityTypeR.Details(id);
        }

        // POST api/<TicketTypeController>
        [HttpPost]
        public async Task<PriorityTypes> Post([FromBody] PriorityTypes value)
        {
            return await _priorityTypeR.Create(value);
        }

        // PUT api/<TicketTypeController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] PriorityTypes value)
        {
             await _priorityTypeR.Edit(value);
        }

        // DELETE api/<TicketTypeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _priorityTypeR.DeleteConfirmed(id);
        }
    }
}
