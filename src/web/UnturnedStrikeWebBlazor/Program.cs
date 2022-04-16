using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using UnturnedStrikeWebBlazor.Providers;
using UnturnedStrikeWebBlazor.Services;

namespace UnturnedStrikeWebBlazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<AuthenticationStateProvider, SteamAuthenticationStateProvider>();


            await builder.Build().RunAsync();
        }
    }
}
