using Autofac;
using Microsoft.AspNet.SignalR;
using Microsoft.Extensions.Configuration;
using RawRabbit;
using RawRabbit.vNext;
using Warden.Common.Events;
using Warden.Common.Events.Wardens;
using Warden.Common.Nancy;
using Warden.Services.Pusher.Handlers;
using Warden.Services.Pusher.Hubs;

namespace Warden.Services.Pusher.Framework
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        private readonly IConfiguration _configuration;
        public static ILifetimeScope LifetimeScope { get; private set; }

        public Bootstrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            base.ConfigureApplicationContainer(container);
            container.Update(builder =>
            {
                builder.RegisterInstance(BusClientFactory.CreateDefault()).As<IBusClient>();
                builder.RegisterInstance(GlobalHost.ConnectionManager.GetHubContext<WardenHub>()).As<IHubContext>();
                builder.RegisterType<SignalRService>().As<ISignalRService>();
                builder.RegisterType<WardenCheckResultProcessedHandler>()
                    .As<IEventHandler<WardenCheckResultProcessed>>();
            });
            LifetimeScope = container;
        }
    }
}