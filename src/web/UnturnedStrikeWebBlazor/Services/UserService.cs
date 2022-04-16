using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UnturnedStrikeAPI;
using UnturnedStrikeAPI.Constants;

namespace UnturnedStrikeWebBlazor.Services
{
    public class UserService
    {
        private readonly HttpClient httpClient;

        public bool IsAuthenticated => CurrentUser?.IsAuthenticated ?? false;

        public bool IsAdmin => CurrentUser?.Role?.Equals(RoleConstants.AdminRoleId) ?? false;

        public bool HasPlayer => CurrentUser?.HasPlayer ?? false;

        public string Balance => Player?.Balance.ToString("C") ?? string.Empty;

        public Player Player => CurrentUser?.Player ?? null;

        public string PlayerId => CurrentUser?.SteamID ?? null;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public UserInfo CurrentUser { get; set; }

        public async Task<UserInfo> GetCurrentUserAsync()
        {
            if (CurrentUser == null)
                CurrentUser = await httpClient.GetFromJsonAsync<UserInfo>("api/user");
            return CurrentUser;
        }

        public async Task<Player> GetCurrentUserPlayerAsync()
        {
            await GetCurrentUserAsync();
            return CurrentUser.Player;
        }
    }
}
