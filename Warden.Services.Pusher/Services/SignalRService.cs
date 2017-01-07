using System;
using Microsoft.AspNetCore.SignalR;
using NLog;
using Warden.Services.Pusher.Hubs;
using Warden.Services.WardenChecks.Shared.Dto;

namespace Warden.Services.Pusher.Services
{
    public class SignalRService : ISignalRService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IHubContext<WardenHub> _hub;

        public SignalRService(IHubContext<WardenHub> hub)
        {
            _hub = hub;
        }

        public void SendCheckResultSaved(Guid organizationId, Guid wardenId, CheckResultDto checkResult)
        {
            var groupName = GetWardenGroupName(organizationId, wardenId);
            Logger.Debug($"Publishing 'check:saved' message.");
            _hub.Clients.All.InvokeAsync("check:saved", checkResult);
            //_hub.Clients.Group(groupName).checkSaved(checkResult);
        }

        private static string GetWardenGroupName(Guid organizationId, Guid wardenId)
            => $"{organizationId:N}:{wardenId:N}";
    }
}