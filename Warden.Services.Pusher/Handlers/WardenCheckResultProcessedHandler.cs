using System.Threading.Tasks;
using Warden.Common.Events;
using Warden.Services.Pusher.Services;
using Warden.Services.WardenChecks.Shared.Events;

namespace Warden.Services.Pusher.Handlers
{
    public class WardenCheckResultProcessedHandler : IEventHandler<WardenCheckResultProcessed>
    {
        private readonly ISignalRService _signalRService;

        public WardenCheckResultProcessedHandler(ISignalRService signalRService)
        {
            _signalRService = signalRService;
        }

        public async Task HandleAsync(WardenCheckResultProcessed @event)
        {
            _signalRService.SendCheckResultSaved(@event.OrganizationId, @event.WardenId, @event.CheckResult);
            await Task.CompletedTask;
        }
    }
}