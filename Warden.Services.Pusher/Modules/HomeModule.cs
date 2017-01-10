namespace Warden.Services.Pusher.Modules
{
    public class HomeModule : ModuleBase
    {
        public HomeModule()
        {
            Get("", args => "Welcome to the Warden.Services.Pusher API!");
        }
    }
}