using System;
using Microsoft.AspNetCore.SignalR;
using NLog;
using Warden.Services.Pusher.Hubs;

namespace Warden.Services.Pusher
{
    public interface ISignalRService
    {
        void SendCheckResultSaved(Guid organizationId, Guid wardenId, object checkResult);
    }

    public class SignalRService : ISignalRService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IHubContext<WardenHub> _hub;

        public SignalRService(IHubContext<WardenHub> hub)
        {
            _hub = hub;
        }

        public void SendCheckResultSaved(Guid organizationId, Guid wardenId, object checkResult)
        {
            var groupName = GetWardenGroupName(organizationId, wardenId);
            Logger.Debug($"Publishing CheckResultSaved message");
            _hub.Clients.All.InvokeAsync("checkSaved", checkResult);
            //_hub.Clients.Group(groupName).checkSaved(checkResult);
        }

        private static string GetWardenGroupName(Guid organizationId, Guid wardenId)
            => $"{organizationId:N}:{wardenId:N}";
    }
}