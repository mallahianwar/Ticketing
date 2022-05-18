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
    public class TicketCommentsController : ControllerBase
    {

        private readonly ICommanRepository<TicketComments> _ticketCommentR;
        private readonly TicketCommentsRepository _ticketComment;

        public TicketCommentsController(ICommanRepository<TicketComments> context,
            TicketCommentsRepository attach)
        {
            _ticketCommentR = context;
            _ticketComment = attach;
          
        }

        // GET: api/<TicketTypeController>
        [HttpGet]
        public async Task<IEnumerable<TicketComments>> Get()
        {
            return await _ticketCommentR.Index();
        }

        // GET api/<TicketTypeController>/5
        [HttpGet("{id}")]
        public async Task<TicketComments> Get(string id)
        {
            return await _ticketCommentR.Details(id);
        }

        [HttpGet("search")]
        public async Task<IEnumerable<TicketComments>> Get([FromBody] TicketVM value)
        {
            return await _ticketComment.SearchVM(value);
        }
        
        // POST api/<TicketTypeController>
        [HttpPost]
        public async Task<TicketComments> Post([FromBody] TicketComments value)
        {
            return await _ticketCommentR.Create(value);
        }

        // PUT api/<TicketTypeController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] TicketComments value)
        {
             await _ticketCommentR.Edit(value);
        }

        // DELETE api/<TicketTypeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _ticketCommentR.DeleteConfirmed(id);
        }
    }
}
