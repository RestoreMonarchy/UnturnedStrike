using Steam.Models.SteamCommunity;
using System.Threading.Tasks;

namespace UnturnedStrikeWeb.Services
{
    public interface ISteamService
    {
        Task<PlayerSummaryModel> GetSteamInfoAsync(string playerId);
    }
}