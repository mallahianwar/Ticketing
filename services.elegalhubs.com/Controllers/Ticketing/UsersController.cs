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
    public class UsersController : ControllerBase
    {

        private readonly ICommanRepository<User> _userR;
        private readonly UsersRepository _user;

        public UsersController(ICommanRepository<User> context,
            UsersRepository user)
        {
            _userR = context;
            _user = user;          
        }

        // GET: api/<TicketTypeController>
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _userR.Index();
        }

        // GET api/<TicketTypeController>/5
        [HttpGet("{id}")]
        public async Task<User> Get(string id)
        {
            return await _userR.Details(id);
        }
        // GET api/<TicketTypeController>/5
        [HttpGet("search")]
        public async Task<IEnumerable<User>> Get([FromBody] UserVM value)
        {
            return await _user.SearchVM(value);;
        }
        // POST api/<TicketTypeController>
        [HttpPost]
        public async Task<User> Post([FromBody] User value)
        {
            return await _userR.Create(value);
        }

        // PUT api/<TicketTypeController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] User value)
        {
             await _userR.Edit(value);
        }

        // DELETE api/<TicketTypeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _userR.DeleteConfirmed(id);
        }
    }
}
