using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRClient
{
    public class Client
    {
        static internal HubConnection connection;

        public static async Task Main(string[] args)
        {
            //  Connect to the SignalR hub.
            connection = new HubConnectionBuilder().WithUrl("http://localhost:9000/hub").Build();
            connection.Closed += Connection_Closed;

            //  Define method(s) to be called from hub.
            connection.On<string>("ReceiveUpdate", (message) =>
            {
                Console.WriteLine($"Received from server: {message}");
            });

            //  Start the connection.
            try
            {
                await connection.StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return;
            }

            //  Call the hub with a message.
            while (true)
            {
                Console.WriteLine("Enter a message (or press ENTER to quit): ");
                string message = Console.ReadLine();
                if (message.Length <= 0)
                {
                    await connection.StopAsync();
                    return;
                }
                if (message[0] == '1')
                {
                    Console.WriteLine("Stopping client");
                    await connection.StopAsync();
                }
                else if (message[0] == '2')
                {
                    Console.WriteLine("Starting client");
                    await connection.StartAsync();
                }
                else
                {
                    await connection.SendAsync("SendUpdate", message);
                }
            }
        }

        private static Task Connection_Closed(Exception arg)
        {
            Console.WriteLine("The server closed the client connection");
            return null;
        }
    }
}
