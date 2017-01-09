using System.Threading.Tasks;
using Warden.Services.Operations.Shared.Events;

namespace Warden.Services.Pusher.Services
{
    public interface IOperationService
    {
         Task PublishOperationUpdatedAsync(OperationUpdated @event);
    }
}