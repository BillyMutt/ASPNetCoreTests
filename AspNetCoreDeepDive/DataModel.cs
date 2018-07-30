using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;

namespace AspNetCoreDeepDive
{
    public class DataModel
    {
        internal Timer timer;
        internal int TotalCount;
        public IHubContext<JohanHub> MyHubContext = null;

        public DataModel()
        {
            TotalCount = 0;
            TimerCallback timer_cb = new TimerCallback(OnTimer);
            timer = new Timer(timer_cb, null, 0, 5000);
        }

        private async void OnTimer(object state)
        {
            string message = $"This is message number {TotalCount++.ToString()}";
            if (MyHubContext != null)
            {
                await MyHubContext.Clients.All.SendAsync("ReceiveUpdate", message);
            }
        }
    }
}
