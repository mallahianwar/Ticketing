using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lms.elegalhubs.com.Helpers
{
    public static class RabbitMQBll
    {
        public static IConnection GetConnection()
        {
            //ConnectionFactory factory = new ConnectionFactory();
            //factory.UserName = "guest";
            //factory.Password = "guest";
            //factory.Port = 5672;
            //factory.HostName = "localhost";
            //factory.VirtualHost = "/";

            var factory = new ConnectionFactory() { Uri = new Uri("amqp://guest:guest@localhost:5672") };

            return factory.CreateConnection();
        }
        public static async Task<bool> send(IConnection con, string message, string friendqueue)
        {
            try
            {
                IModel channel = con.CreateModel();
                channel.ExchangeDeclare("messageexchange", ExchangeType.Direct);
                channel.QueueDeclare(friendqueue, true, false, false, null);
                channel.QueueBind(friendqueue, "messageexchange", friendqueue, null);
                var msg = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("messageexchange", friendqueue, null, msg);

            }
            catch (Exception)
            {


            }
            return true;

        }
        public static async Task<string> receive(IConnection con, string myqueue)
        {
            try
            {
                string queue = myqueue;
                IModel channel = con.CreateModel();
                channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                BasicGetResult result = channel.BasicGet(queue: queue, autoAck: true);
                if (result != null)
                {
                    var body = result.Body.ToArray();
                    var msg = Encoding.UTF8.GetString(body);
                    return  msg;
                }                   
                else
                    return null;


            }
            catch (Exception)
            {
                return null;

            }

        }        
        public static async Task<string> receiveMQ()
        {
            try
            {
                //RabbitMQBll obj = new RabbitMQBll();
                IConnection con = GetConnection();
                //string queue = myqueue;
                IModel channel = con.CreateModel();
                channel.QueueDeclare(queue: "Notification1", durable: true, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                BasicGetResult result = channel.BasicGet(queue: "Notification1", autoAck: true);
                if (result != null)
                {
                    var body = result.Body.ToArray();
                    var msg = Encoding.UTF8.GetString(body);
                    MessageHub hub = new MessageHub();
                    await hub.SendMQMessage("", msg);
                    return  msg;
                }                   
                else
                    return null;


            }
            catch (Exception)
            {
                return null;

            }

        }
    }
}
