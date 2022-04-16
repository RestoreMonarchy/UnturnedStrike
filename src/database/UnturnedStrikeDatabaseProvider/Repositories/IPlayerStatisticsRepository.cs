using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrikeAPI;
using UnturnedStrikeAPI.Enumerators;
using UnturnedStrikeAPI.Params;

namespace UnturnedStrikeDatabaseProvider.Repositories
{
    public interface IPlayerStatisticsRepository : IRepository
    {
        Task<Player> GetPlayerStatisticsAsync(string playerId);
        Task<IEnumerable<Player>> GetTopPlayersAsync(PlayerStatisticsParams playerStatisticsParams);
    }
}
