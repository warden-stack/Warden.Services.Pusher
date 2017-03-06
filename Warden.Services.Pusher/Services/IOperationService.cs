using System.Threading.Tasks;
using Warden.Messages.Events.Operations;

namespace Warden.Services.Pusher.Services
{
    public interface IOperationService
    {
         Task PublishOperationUpdatedAsync(OperationUpdated @event);
    }
}