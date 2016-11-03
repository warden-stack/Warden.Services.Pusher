using Warden.Common.Events.Wardens;
using Warden.Common.Host;
using Warden.Services.Pusher.Framework;

namespace Warden.Services.Pusher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(port: 5054)
                .UseAutofac(Bootstrapper.LifetimeScope)
                .UseRabbitMq()
                .SubscribeToEvent<WardenCheckResultProcessed>()
                .Build()
                .Run();
        }
    }
}
