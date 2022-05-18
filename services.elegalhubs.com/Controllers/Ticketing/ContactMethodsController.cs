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
    public class ContactMethodsController : ControllerBase
    {

        private readonly ICommanRepository<ContactMethods> _contactMethodR;

        public ContactMethodsController(ICommanRepository<ContactMethods> context)
        {
            _contactMethodR = context;
          
        }

        // GET: api/<TicketTypeController>
        //[HttpGet]
        public async Task<IEnumerable<ContactMethods>> Get()
        {
            return await _contactMethodR.Index();
        }

        // GET api/<TicketTypeController>/5
        [HttpGet("{id}")]
        public async Task<ContactMethods> Get(string id)
        {
            return await _contactMethodR.Details(id);
        }

        // POST api/<TicketTypeController>
        [HttpPost]
        public async Task<ContactMethods> Post([FromBody] ContactMethods value)
        {
            return await _contactMethodR.Create(value);
        }

        // PUT api/<TicketTypeController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] ContactMethods value)
        {
             await _contactMethodR.Edit(value);
        }

        // DELETE api/<TicketTypeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _contactMethodR.DeleteConfirmed(id);
        }
    }
}
