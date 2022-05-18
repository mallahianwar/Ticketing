using library.elegalhubs.com.Notification;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microserver_publisher.Services
{
    public class NotificationService
    {
        public static void RabbitNotification(Notifications notification)
        {
            //var factory = new ConnectionFactory
            //{
            //    Uri = new Uri("amqp://guest:guest@localhost:5672")
            //};
            //using var connection = factory.CreateConnection();
            //using var channel = connection.CreateModel();

            //channel.QueueDeclare("Notification",
            //      durable: true,
            //      exclusive: false,
            //      autoDelete: false,
            //      arguments: null);

            //var message = notification;
            //var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            //channel.BasicPublish("", "Notification", null, body);

            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Notification1",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                //string message = "Hello World!";
                var message = notification;

                // var body = Encoding.UTF8.GetBytes(message);
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));


                channel.BasicPublish(exchange: "",
                                     routingKey: "Notification1",
                                     basicProperties: null,
                                     body: body);
            }
        }

    }
}
