using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SteamQueryNet;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnturnedStrikeAPI;
using UnturnedStrikeDatabaseProvider.Repositories;

namespace UnturnedStrikeWeb.Services
{
    public class ServersStatusService : IHostedService
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

        private CancellationTokenSource cancellationTokenSource;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => PeriodicRefreshAsync(), cancellationTokenSource.Token);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }

        private async Task PeriodicRefreshAsync()
        {
            while (true)
            {
                try
                {
                    await RefreshAsync();
                }
                catch (Exception e)
                {
                    logger.LogError(e, "");
                }

                await Task.Delay(12000, cancellationTokenSource.Token);
            }
        }

        public async Task RefreshAsync()
        {
            var servers = await gameServersRepository.GetServersAsync();
            foreach (var server in servers)
            {
                var query = new ServerQuery
                {
                    ReceiveTimeout = 500,
                    SendTimeout = 500
                };

                try
                {
                    query.Connect(server.Address, (ushort)(server.Port + 1));
                    var info = await query.GetServerInfoAsync();
                    server.Info = new GameServerInfo()
                    {
                        Name = info.Name,
                        Players = info.Players,
                        MaxPlayers = info.MaxPlayers,
                        Map = info.Map
                    };
                }
                catch (TimeoutException)
                {

                }
            }

            memoryCache.Set("Servers", servers);
        }
    }
}
