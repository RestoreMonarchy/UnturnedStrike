using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories
{
    public interface IGameServersRepository : IRepository
    {
        Task<IEnumerable<GameServer>> GetServersAsync();
    }
}
