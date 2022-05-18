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
    public class CategoryTypesController : ControllerBase
    {

        private readonly ICommanRepository<CategoryTypes> _categoryTypeR;

        public CategoryTypesController(ICommanRepository<CategoryTypes> context)
        {
            _categoryTypeR = context;
          
        }

        // GET: api/<TicketTypeController>
        //[HttpGet]
        public async Task<IEnumerable<CategoryTypes>> Get()
        {
            return await _categoryTypeR.Index();
        }

        // GET api/<TicketTypeController>/5
        [HttpGet("{id}")]
        public async Task<CategoryTypes> Get(string id)
        {
            return await _categoryTypeR.Details(id);
        }

        // POST api/<TicketTypeController>
        [HttpPost]
        public async Task<CategoryTypes> Post([FromBody] CategoryTypes value)
        {
            return await _categoryTypeR.Create(value);
        }

        // PUT api/<TicketTypeController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] CategoryTypes value)
        {
             await _categoryTypeR.Edit(value);
        }

        // DELETE api/<TicketTypeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _categoryTypeR.DeleteConfirmed(id);
        }
    }
}
