using System.Threading.Tasks;
using UnturnedStrikeAPI;
using UnturnedStrikeDatabaseProvider.Repositories;

namespace UnturnedStrikeWeb.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayersRepository playersRepository;
        private readonly ISteamService steamService;
        private readonly ICountryService countryService;

        public PlayerService(IPlayersRepository playersRepository, ISteamService steamService, ICountryService countryService)
        {
            this.playersRepository = playersRepository;
            this.steamService = steamService;
            this.countryService = countryService;
        }

        public async Task<Player> UpdatePlayerAsync(Player player)
        {
            var steamPlayer = await steamService.GetSteamInfoAsync(player.Id);

            player.SteamName = steamPlayer.Nickname;
            player.SteamIconUrl = steamPlayer.AvatarFullUrl;
            if (player.IP != null)
                player.Country = await countryService.GetCountryAsync(player.IP);
            return await playersRepository.UpdatePlayerAsync(player);
        }
    }
}
