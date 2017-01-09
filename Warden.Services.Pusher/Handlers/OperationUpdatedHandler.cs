using System;
using System.Threading.Tasks;
using NLog;
using Warden.Common.Events;
using Warden.Services.Operations.Shared.Events;
using Warden.Services.Pusher.Services;

namespace Warden.Services.Pusher.Handlers
{
    public class OperationUpdatedHandler : IEventHandler<OperationUpdated>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IOperationService _operationService;

        public OperationUpdatedHandler(IOperationService operationService)
        {
            _operationService = operationService;
        }

        public async Task HandleAsync(OperationUpdated @event)
        {
            try
            {
                await _operationService.PublishOperationUpdatedAsync(@event);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}