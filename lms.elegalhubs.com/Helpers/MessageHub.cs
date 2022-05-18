using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace lms.elegalhubs.com.Helpers
{
    public class MessageHub : Hub
    {
        public static List<string> users = new List<string>();

        public override Task OnConnectedAsync()
        {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            users.Add(userId);
            Groups.AddToGroupAsync(Context.ConnectionId, userId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            users.Remove(userId);
            return base.OnDisconnectedAsync(exception);

        }
        public async Task SendMessage(string user, string message)
        {
            if (string.IsNullOrEmpty(user))
                await Clients.All.SendAsync("ReceiveMessageHandler", message);
            else
                await Clients.User(user).SendAsync("ReceiveMessageHandler", message);
        }
        public async Task SendMQMessage(string user, string message)
        {
            if (string.IsNullOrEmpty(user))
                await Clients.All.SendAsync("ReceiveMQMessageHandler", message);
            else
                await Clients.User(user).SendAsync("ReceiveMQMessageHandler", message);
        }
        public Task SendMessageToGroup(string sender, string receiver, string message)
        {
            return Clients.Group(receiver).SendAsync("ReceiveMessage", sender, message);
        }
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
        //public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        //{
        //    var connection = CurrentConnections.FirstOrDefault(x => x == Context.ConnectionId);

        //    if (connection != null)
        //    {
        //        CurrentConnections.Remove(connection);
        //    }

        //    return base.OnDisconnected(stopCalled);
        //}


        //return list of all active connections
        public List<string> GetAllActiveConnections()
        {
            //Clients.Groups.Context;
            var m = GetAllActiveConnections();
                return m;
        }

    }
}
