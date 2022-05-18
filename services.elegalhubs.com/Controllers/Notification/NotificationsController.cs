using library.elegalhubs.com.Notification;
using Microserver_publisher.Services;
using Microsoft.AspNetCore.Mvc;
using services.elegalhubs.com.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace services.elegalhubs.com.Controllers.Notification
{
   

    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ICommanRepository<Notifications> _commanRepository;

        public NotificationsController(ICommanRepository<Notifications> context)
        {
            _commanRepository = context;

        }

        // GET: api/<NotificationController>
        public async Task<IEnumerable<Notifications>> Get()
        {
            return await _commanRepository.Index();
        }

        // GET api/<NotificationController>/5
        [HttpGet("{id}")]
        public async Task<Notifications> Get(string id)
        {
            return await _commanRepository.Details(id);
        }

        // POST api/<NotificationController>
        [HttpPost]
        public async Task<Notifications> Post([FromBody] Notifications value)
        {
            return await _commanRepository.Create(value);
        }

        // PUT api/<NotificationController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Notifications value)
        {
            await _commanRepository.Edit(value);
        }

        // DELETE api/<NotificationController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _commanRepository.DeleteConfirmed(id);
        }

    }
}
