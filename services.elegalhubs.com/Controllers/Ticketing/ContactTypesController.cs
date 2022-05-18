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
    public class ContactTypesController : ControllerBase
    {

        private readonly ICommanRepository<ContactTypes> _contactTypeR;

        public ContactTypesController(ICommanRepository<ContactTypes> context)
        {
            _contactTypeR = context;
          
        }

        // GET: api/<TicketTypeController>
        //[HttpGet]
        public async Task<IEnumerable<ContactTypes>> Get()
        {
            return await _contactTypeR.Index();
        }

        // GET api/<TicketTypeController>/5
        [HttpGet("{id}")]
        public async Task<ContactTypes> Get(string id)
        {
            return await _contactTypeR.Details(id);
        }

        // POST api/<TicketTypeController>
        [HttpPost]
        public async Task<ContactTypes> Post([FromBody] ContactTypes value)
        {
            return await _contactTypeR.Create(value);
        }

        // PUT api/<TicketTypeController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] ContactTypes value)
        {
             await _contactTypeR.Edit(value);
        }

        // DELETE api/<TicketTypeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _contactTypeR.DeleteConfirmed(id);
        }
    }
}
