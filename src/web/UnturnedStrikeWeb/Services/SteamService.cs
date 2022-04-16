using Steam.Models.SteamCommunity;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnturnedStrikeWeb.Services
{
    public class SteamService : ISteamService
    {
        private readonly ISteamWebInterfaceFactory steamWebInterfaceFactory;
        private readonly IHttpClientFactory httpClientFactory;

        public SteamService(ISteamWebInterfaceFactory steamWebInterfaceFactory, IHttpClientFactory httpClientFactory)
        {
            this.steamWebInterfaceFactory = steamWebInterfaceFactory;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<PlayerSummaryModel> GetSteamInfoAsync(string playerId)
        {
            if (!ulong.TryParse(playerId, out ulong steamId))
            {
                return null;
            }

            var steamUser = steamWebInterfaceFactory.CreateSteamWebInterface<SteamUser>(httpClientFactory.CreateClient());
            var response = await steamUser.GetPlayerSummaryAsync(steamId);
            return response.Data;
        }
    }
}
