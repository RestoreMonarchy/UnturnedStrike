using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories
{
    public interface IPlayersRepository : IRepository
    {
        Task<IEnumerable<Player>> GetPlayersAsync();
        Task<Player> GetPlayerAsync(string steamId);
        Task CreatePlayerAsync(string steamId);
        Task<Player> UpdatePlayerAsync(Player player);
        Task AddToPlayerBalanceAsync(string steamId, decimal amount);
    }
}
