using Warden.Common.Host;
using Warden.Messages.Events.Operations;
using Warden.Messages.Events.WardenChecks;

namespace Warden.Services.Pusher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(port: 5054)
                .UseAutofac(Startup.LifetimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToEvent<WardenCheckResultProcessed>()
                .SubscribeToEvent<OperationUpdated>()
                .Build()
                .Run();
        }
    }
}
