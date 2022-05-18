//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace lms.elegalhubs.com.Helpers
//{
//    public class PCR
//    {
//    }
//    class RPCServer
//    {
//        public static void Main()
//        {
//            var factory = new ConnectionFactory() { HostName = "localhost" };
//            using (var connection = factory.CreateConnection())
//            using (var channel = connection.CreateModel())
//            {
//                channel.QueueDeclare(queue: "rpc_queue", durable: false,
//                  exclusive: false, autoDelete: false, arguments: null);
//                channel.BasicQos(0, 1, false);
//                var consumer = new EventingBasicConsumer(channel);
//                channel.BasicConsume(queue: "rpc_queue",
//                  autoAck: false, consumer: consumer);

//                consumer.Received += (model, ea) =>
//                {
//                    string response = null;

//                    var body = ea.Body.ToArray();
//                    var props = ea.BasicProperties;
//                    var replyProps = channel.CreateBasicProperties();
//                    replyProps.CorrelationId = props.CorrelationId;

//                    try
//                    {
//                        var message = Encoding.UTF8.GetString(body);
//                        int n = int.Parse(message);
//                        Console.WriteLine(" [.] fib({0})", message);
//                        response = fib(n).ToString();
//                    }
//                    catch (Exception e)
//                    {
//                        Console.WriteLine(" [.] " + e.Message);
//                        response = "";
//                    }
//                    finally
//                    {
//                        var responseBytes = Encoding.UTF8.GetBytes(response);
//                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
//                          basicProperties: replyProps, body: responseBytes);
//                        channel.BasicAck(deliveryTag: ea.DeliveryTag,
//                          multiple: false);
//                    }
//                };

//            }
//        }

//        /// 

//        /// Assumes only valid positive integer input.
//        /// Don't expect this one to work for big numbers, and it's
//        /// probably the slowest recursive implementation possible.
//        /// 

//        private static int fib(int n)
//        {
//            if (n == 0 || n == 1)
//            {
//                return n;
//            }

//            return fib(n - 1) + fib(n - 2);
//        }
//    }

//    public class RpcClient
//    {
//        private readonly IConnection connection;
//        private readonly IModel channel;
//        private readonly string replyQueueName;
//        private readonly EventingBasicConsumer consumer;
//        private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
//        private readonly IBasicProperties props;

//        public RpcClient()
//        {
//            var factory = new ConnectionFactory() { HostName = "localhost" };

//            connection = factory.CreateConnection();
//            channel = connection.CreateModel();
//            replyQueueName = channel.QueueDeclare().QueueName;
//            consumer = new EventingBasicConsumer(channel);

//            props = channel.CreateBasicProperties();
//            var correlationId = Guid.NewGuid().ToString();
//            props.CorrelationId = correlationId;
//            props.ReplyTo = replyQueueName;

//            consumer.Received += (model, ea) =>
//            {
//                var body = ea.Body.ToArray();
//                var response = Encoding.UTF8.GetString(body);
//                if (ea.BasicProperties.CorrelationId == correlationId)
//                {
//                    respQueue.Add(response);
//                }
//            };

//            channel.BasicConsume(
//                consumer: consumer,
//                queue: replyQueueName,
//                autoAck: true);
//        }

//        public string Call(string message)
//        {
//            var messageBytes = Encoding.UTF8.GetBytes(message);
//            channel.BasicPublish(
//                exchange: "",
//                routingKey: "rpc_queue",
//                basicProperties: props,
//                body: messageBytes);

//            return respQueue.Take();
//        }

//        public void Close()
//        {
//            connection.Close();
//        }
//    }

//    //public class Rpc
//    //{
//    //    public static void Main()
//    //    {
//    //        var rpcClient = new RpcClient();

//    //        Console.WriteLine(" [x] Requesting fib(30)");
//    //        var response = rpcClient.Call("30");

//    //        Console.WriteLine(" [.] Got '{0}'", response);
//    //        rpcClient.Close();
//    //    }
//    //}
//}
