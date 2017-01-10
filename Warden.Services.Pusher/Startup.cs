using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Lockbox.Client.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using RawRabbit.Configuration;
using Warden.Common.Commands;
using Warden.Common.Events;
using Warden.Common.Extensions;
using Warden.Common.RabbitMq;
using Warden.Common.Security;
using Warden.Services.Pusher.Hubs;
using Warden.Services.Pusher.Services;

namespace Warden.Services.Pusher
{
    public class Startup
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public IConfiguration Configuration { get; set; }
        public static ILifetimeScope LifetimeScope { get; private set; }
                //TODO: get rid of static token handler after update signalr to newer version
        public static IJwtTokenHandler JwtTokenHandler { get; private set; }

        
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .SetBasePath(env.ContentRootPath);
                
            if (env.IsProduction())
            {
                builder.AddLockbox();
            }

            Configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddCors();
            services.AddSignalR();

            var assembly = Assembly.GetEntryAssembly();
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IEventHandler<>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterType<CheckResultService>().As<ICheckResultService>();
            builder.RegisterType<OperationService>().As<IOperationService>();
            SecurityContainer.Register(builder, Configuration);
            RabbitMqContainer.Register(builder, Configuration.GetSettings<RawRabbitConfiguration>());
            LifetimeScope = builder.Build().BeginLifetimeScope();
            JwtTokenHandler = LifetimeScope.Resolve<IJwtTokenHandler>();

            return new AutofacServiceProvider(LifetimeScope);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, IApplicationLifetime appLifeTime)
        {
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");
            app.UseCors(builder => builder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials());
            app.UseSignalR(builder => builder.MapHub<WardenHub>("/hub"));
            appLifeTime.ApplicationStopped.Register(() => LifetimeScope.Dispose());
        }
    }
}