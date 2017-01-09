using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Warden.Services.Pusher.Hubs;
using Warden.Services.WardenChecks.Shared.Dto;

namespace Warden.Services.Pusher.Services
{
    public class CheckResultService : ICheckResultService
    {
        private readonly IHubContext<WardenHub> _hubContext;

        public CheckResultService(IHubContext<WardenHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task PublishCheckResultCreatedAsync(Guid organizationId, Guid wardenId, CheckResultDto checkResult)
        {
            await _hubContext
                .Clients
                .All
                .InvokeAsync("check_result_created", checkResult);
        }

        private static string GetWardenGroupName(Guid organizationId, Guid wardenId)
            => $"{organizationId:N}:{wardenId:N}";
    }
}