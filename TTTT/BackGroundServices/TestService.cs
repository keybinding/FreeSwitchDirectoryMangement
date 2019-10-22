using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TTTT.Model;
using TTTT.Persistent;
using TTTT.Persistent.Repositories;
using TTTT.Services;

namespace TTTT.BackGroundServices
{
    public class TestService : BackgroundService
    {
        protected readonly IServiceScopeFactory scopeFactory;
        public TestService(IServiceScopeFactory a_scopeFactory) : base()
        {
            scopeFactory = a_scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(10000);
            var factory = new ConnectionFactory() { HostName = "rabbit", Port = 5672, UserName = "rabbitmq", Password = "rabbitmq", VirtualHost = "/" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "createUser",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    User user = JsonConvert.DeserializeObject(message, typeof(User)) as User;
                    using (var scope = scopeFactory.CreateScope())
                    {
                        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                        try
                        {
                            var v = await userService.AddAsync(user);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                };
                channel.BasicConsume(queue: "createUser",
                                     autoAck: true,
                                     consumer: consumer);
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }
    }
}
