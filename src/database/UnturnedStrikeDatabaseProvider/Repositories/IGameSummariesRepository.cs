using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories
{
    public interface IGameSummariesRepository : IRepository
    {
        Task<GameSummary> CreateGameSummary(GameSummary gameSummary);
        Task UpdateGameSummary(GameSummary gameSummary);
    }
}
