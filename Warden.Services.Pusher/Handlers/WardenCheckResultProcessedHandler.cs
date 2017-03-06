using System.Threading.Tasks;
using Warden.Messages.Events;
using Warden.Services.Pusher.Services;
using Warden.Messages.Events.WardenChecks;

namespace Warden.Services.Pusher.Handlers
{
    public class WardenCheckResultProcessedHandler : IEventHandler<WardenCheckResultProcessed>
    {
        private readonly ICheckResultService _checkResultService;

        public WardenCheckResultProcessedHandler(ICheckResultService checkResultService)
        {
            _checkResultService = checkResultService;
        }

        public async Task HandleAsync(WardenCheckResultProcessed @event)
        {
            await _checkResultService.PublishCheckResultCreatedAsync(@event.OrganizationId, @event.WardenId, @event.CheckResult);
        }
    }
}