using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreDeepDive
{
    public class JohanHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("Hub client connected");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("Hub client disconnected");
            await base.OnDisconnectedAsync(exception);
        }

        //  Our hub method for pushing updates to connected clients.
        public async Task SendUpdate(string message)
        {
            Console.WriteLine($"Hub client called SendUpdate() with message: {message}");
            await Clients.All.SendAsync("ReceiveUpdate", message);
        }
    }
}
