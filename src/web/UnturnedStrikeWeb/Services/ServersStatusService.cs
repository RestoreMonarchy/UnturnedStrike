using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SteamServerQuery;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnturnedStrikeAPI;
using UnturnedStrikeDatabaseProvider.Repositories;

namespace UnturnedStrikeWeb.Services
{
    public class ServersStatusService
    {
        private readonly IMemoryCache memoryCache;
        private readonly IGameServersRepository gameServersRepository;
        private readonly ILogger<ServersStatusService> logger;

        public ServersStatusService(IMemoryCache memoryCache, IGameServersRepository gameServersRepository, ILogger<ServersStatusService> logger)
        {
            this.memoryCache = memoryCache;
            this.gameServersRepository = gameServersRepository;
            this.logger = logger;
        }

        public async Task<IEnumerable<GameServer>> GetGameServersFromCacheAsync()
        {
            return await memoryCache.GetOrCreateAsync("Servers", entry => 
            {
                entry.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10);
                return GetGameServersAsync();
            });
        }

        public async Task<IEnumerable<GameServer>> GetGameServersAsync()
        {
            IEnumerable<GameServer> gameServers = await gameServersRepository.GetServersAsync();

            foreach (GameServer gameServer in gameServers)
            {
                gameServer.Info = await GetServerInfoAsync(gameServer.Address, gameServer.Port);
            }

            return gameServers;
        }

        private async Task<GameServerInfo> GetServerInfoAsync(string address, int port)
        {
            ServerInfo info = await SteamServer.QueryServerAsync(address, port, 1000);

            return new GameServerInfo()
            {
                Name = info.Name,
                Players = (byte)info.Players,
                MaxPlayers = (byte)info.MaxPlayers,
                Map = info.Map
            };
        }
    }
}
