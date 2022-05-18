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
    public class ProjectsController : ControllerBase
    {

        private readonly ICommanRepository<Projects> _projects;

        public ProjectsController(ICommanRepository<Projects> context)
        {
            _projects = context;
          
        }

        // GET: api/<TicketTypeController>
        [HttpGet]
        public async Task<IEnumerable<Projects>> Get()
        {
            return await _projects.Index();
        }

        // GET api/<TicketTypeController>/5
        [HttpGet("{id}")]
        public async Task<Projects> Get(string id)
        {
            return await _projects.Details(id);
        }

        // POST api/<TicketTypeController>
        [HttpPost]
        public async Task<Projects> Post([FromBody] Projects value)
        {
            return await _projects.Create(value);
        }

        // PUT api/<TicketTypeController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Projects value)
        {
             await _projects.Edit(value);
        }

        // DELETE api/<TicketTypeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _projects.DeleteConfirmed(id);
        }
    }
}
