using System;
using System.Reflection;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using RawRabbit.Configuration;
using Warden.Common.Commands;
using Warden.Common.Events;
using Warden.Common.Extensions;
using Warden.Common.RabbitMq;
using Warden.Services.Pusher.Hubs;

namespace Warden.Services.Pusher
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public static ILifetimeScope LifetimeScope { get; private set; }
        
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .SetBasePath(env.ContentRootPath);

            Configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddCors();
            services.AddSignalR();

            var assembly = Assembly.GetEntryAssembly();
            var builder = new ContainerBuilder();
            RabbitMqContainer.Register(builder, Configuration.GetSettings<RawRabbitConfiguration>());
            builder.Populate(services);
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IEventHandler<>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterType<SignalRService>().As<ISignalRService>();

            LifetimeScope = builder.Build().BeginLifetimeScope();

            return new AutofacServiceProvider(LifetimeScope);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");
            app.UseCors(builder => builder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials());
            app.UseSignalR(builder => builder.MapHub<WardenHub>("/hub"));
        }
    }
}