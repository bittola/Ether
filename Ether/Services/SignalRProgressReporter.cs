﻿using Ether.Core.Interfaces;
using Ether.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Ether.Services
{
    public class SignalRProgressReporter : IProgressReporter
    {
        private readonly IHubContext<LiveUpdatesHub> _hub;

        public SignalRProgressReporter(IHubContext<LiveUpdatesHub> hub)
        {
            _hub = hub;
        }

        public async Task Report(string message, float moveProgressBy = 0)
        {
            await _hub.Clients.All.SendAsync("update", message, moveProgressBy);
        }

        public async Task Reset()
        {
            await _hub.Clients.All.SendAsync("reset");
        }
    }
}
