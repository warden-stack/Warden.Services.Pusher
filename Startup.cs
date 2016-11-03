using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Cors;
using Nancy.Owin;
using NLog.Extensions.Logging;
using Owin;
using Warden.Services.Pusher.Framework;

namespace Warden.Services.Pusher
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .SetBasePath(env.ContentRootPath);

            Configuration = builder.Build();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");
            app.UseOwin().UseNancy(x => x.Bootstrapper = new Bootstrapper(Configuration));
            app.UseOwin(addToPipeline =>
            {
                addToPipeline(next =>
                {
                    var appBuilder = new AppBuilder();
                    appBuilder.Properties["builder.DefaultApp"] = next;
                    appBuilder.UseCors(CorsOptions.AllowAll);
                    appBuilder.MapSignalR();

                    return appBuilder.Build();
                });
            });
        }
    }
}