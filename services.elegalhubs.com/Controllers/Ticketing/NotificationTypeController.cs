using library.elegalhubs.com.Notification;
using Microserver_publisher.Services;
using Microsoft.AspNetCore.Mvc;
using services.elegalhubs.com.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace services.elegalhubs.com.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationTypeController : ControllerBase
    {
        private readonly ICommanRepository<NotificationTypes> _commanRepository;

        public NotificationTypeController(ICommanRepository<NotificationTypes> context)
        {
            _commanRepository = context;

        }

        // GET api/<NotificationTypeController>/5
        [HttpGet("{id}")]
        public async Task<NotificationTypes> Get(string id)
        {
            return await _commanRepository.Details(id);
        }

        // POST api/<NotificationTypeController>
        [HttpPost]
        public async Task<NotificationTypes> Post([FromBody] NotificationTypes value)
        {
            return await _commanRepository.Create(value);
        }

        // PUT api/<NotificationTypeController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] NotificationTypes value)
        {
            await _commanRepository.Edit(value);
        }

        // DELETE api/<NotificationTypeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _commanRepository.DeleteConfirmed(id);
        }
    }
}
