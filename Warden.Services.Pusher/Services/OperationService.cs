using System.Threading.Tasks;
using Warden.Services.Operations.Shared.Events;
using Warden.Services.Pusher.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Warden.Services.Pusher.Services
{
    public class OperationService : IOperationService
    {
        private readonly IHubContext<WardenHub> _hubContext;

        public OperationService(IHubContext<WardenHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task PublishOperationUpdatedAsync(OperationUpdated @event)
        {
            var message = new
            {
                requestId = @event.RequestId.ToString("N"),
                name = @event.Name,
                userId = @event.UserId,
                state = @event.State,
                code = @event.Code,
                message = @event.Message,
                updatedAt = @event.UpdatedAt
            };

            await _hubContext
                .Clients
                .Group(@event.UserId)
                .InvokeAsync("operation_updated", message);
        } 
    }
}