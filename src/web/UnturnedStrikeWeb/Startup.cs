using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SteamWebAPI2.Utilities;
using System;
using System.Data.SqlClient;
using UnturnedStrikeDatabaseProvider.Repositories;
using UnturnedStrikeDatabaseProvider.Repositories.Sql;
using UnturnedStrikeWeb.Extensions;
using UnturnedStrikeWeb.Helpers;
using UnturnedStrikeWeb.Services;

namespace UnturnedStrikeWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddHttpClient();

            services.AddAuthentication(options => { options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; })
                .AddCookie(options =>
                {
                    options.Cookie.SameSite = SameSiteMode.Unspecified;
                    options.LoginPath = "/signin";
                    options.LogoutPath = "/signout";
                    options.AccessDeniedPath = "/";
                    options.Events.OnValidatePrincipal = ValidationHelper.Validate;
                    options.ExpireTimeSpan = TimeSpan.FromHours(12);
                }).AddSteam(x => x.ApplicationKey = Configuration["SteamAPIKey"]);

            services.AddAuthorizationCore();

            services.AddTransient(x => new SqlConnection(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<ISteamWebInterfaceFactory>(x => new SteamWebInterfaceFactory(Configuration["SteamAPIKey"]));

            services.AddTransient<IPlayersRepository, PlayersRepository>();
            services.AddTransient<IGameSummariesRepository, GameSummariesRepository>();
            services.AddTransient<IGameServersRepository, GameServersRepository>();
            services.AddTransient<IPlayerStatisticsRepository, PlayerStatisticsRepository>();
            services.AddTransient<ITransactionsRepository, TransactionsRepository>();
            services.AddTransient<IFilesRepository, FilesRepository>();
            services.AddTransient<IWeaponsRepository, WeaponsRepository>();
            services.AddTransient<IWeaponSkinsRepository, WeaponSkinsRepository>();
            services.AddTransient<IPurchasesRepository, PurchasesRepository>();
            services.AddTransient<IBoxesRepository, BoxesRepository>();

            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<ISteamService, SteamService>();
            services.AddTransient<IPlayerService, PlayerService>();
            services.AddTransient<ITransactionsService, TransactionsService>();
            services.AddTransient<IPurchasesService, PurchasesService>();

            services.AddTransient<ServersStatusService>();

            services.AddSameSiteCookiePolicy();

            services.AddControllers();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCookiePolicy();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
