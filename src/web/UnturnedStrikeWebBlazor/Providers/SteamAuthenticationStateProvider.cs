using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using UnturnedStrikeAPI;
using UnturnedStrikeWebBlazor.Services;

namespace UnturnedStrikeWebBlazor.Providers
{
    public class SteamAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly UserService userService;

        public SteamAuthenticationStateProvider(UserService userService)
        {
            this.userService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userInfo = await userService.GetCurrentUserAsync();
            ClaimsIdentity steamIdentity;

            if (userInfo.IsAuthenticated)
            {
                steamIdentity = new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, userInfo.SteamID),
                    new Claim(ClaimTypes.Role, userInfo.Role)
                }, "SteamAuth");
            }
            else
            {
                steamIdentity = new ClaimsIdentity();
            }

            return new AuthenticationState(new ClaimsPrincipal(steamIdentity));
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
