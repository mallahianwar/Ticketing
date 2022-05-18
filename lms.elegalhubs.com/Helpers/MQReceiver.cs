using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace lms.elegalhubs.com.Helpers
{
    public class MQReceiver : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            stoppingToken.ThrowIfCancellationRequested();

            _ = RabbitMQBll.receiveMQ();

            return Task.CompletedTask;

            //throw new NotImplementedException();
        }
    }
}
