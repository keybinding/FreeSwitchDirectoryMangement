using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using TTTT.Services;
using TTTT.Model;
using System.Reflection;
using Newtonsoft.Json;

namespace TTTT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        protected readonly IUserService userService;

        public ValuesController(IUserService a_userService)
        {
            userService = a_userService;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await userService.ListAsync();
            return users;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return id.ToString();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] User a_user)
        {
            PostUserCreateRequest(a_user);
            return Ok();
        }

        public void PostUserCreateRequest(User a_user)
        {
            var factory = new ConnectionFactory() { HostName = "rabbit", Port = 5672, UserName = "rabbitmq", Password = "rabbitmq", VirtualHost = "/" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "createUser",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                string message = JsonConvert.SerializeObject(a_user);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "createUser",
                                     basicProperties: null,
                                     body: body);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        
    }
}
