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
    public class StatusTypesController : ControllerBase
    {

        private readonly ICommanRepository<StatusTypes> _statusTypeR;

        public StatusTypesController(ICommanRepository<StatusTypes> context)
        {
            _statusTypeR = context;
          
        }

        // GET: api/<TicketTypeController>
        //[HttpGet]
        public async Task<IEnumerable<StatusTypes>> Get()
        {
            return await _statusTypeR.Index();
        }

        // GET api/<TicketTypeController>/5
        [HttpGet("{id}")]
        public async Task<StatusTypes> Get(string id)
        {
            return await _statusTypeR.Details(id);
        }

        // POST api/<TicketTypeController>
        [HttpPost]
        public async Task<StatusTypes> Post([FromBody] StatusTypes value)
        {
            return await _statusTypeR.Create(value);
        }

        // PUT api/<TicketTypeController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] StatusTypes value)
        {
             await _statusTypeR.Edit(value);
        }

        // DELETE api/<TicketTypeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _statusTypeR.DeleteConfirmed(id);
        }
    }
}
