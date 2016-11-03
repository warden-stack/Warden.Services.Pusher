using Nancy;

namespace Warden.Services.Pusher.Modules
{
    public class HomeModule : ModuleBase
    {
        public HomeModule()
        {
            Get("", args => Response.AsJson(new { name = "Warden.Services.Pusher" }));
        }
    }
}