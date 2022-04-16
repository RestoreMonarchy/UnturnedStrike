using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UnturnedStrikeAPI;
using UnturnedStrikeAPI.Params;

namespace UnturnedStrikeDatabaseProvider.Repositories.Sql
{
    public class PlayerStatisticsRepository : IPlayerStatisticsRepository
    {
        private readonly SqlConnection connection;

        public PlayerStatisticsRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<Player>> GetTopPlayersAsync(PlayerStatisticsParams playerStatisticsParams)
        {
            const string sql = "dbo.GetPlayersStatistics";
            return await connection.QueryAsync<Player, PlayerStatistic, Player>(sql, (p, s) =>
            {
                p.Statistic = s;
                return p;
            }, new { 
                playerStatisticsParams.DateFrom, 
                playerStatisticsParams.DateTo, 
                playerStatisticsParams.Top,
                playerStatisticsParams.OrderBy
            }, splitOn: "IsVIP", commandType: CommandType.StoredProcedure);
        }

        public async Task<Player> GetPlayerStatisticsAsync(string playerId)
        {
            const string sql = "SELECT p.*, s.* FROM dbo.Players p JOIN dbo.PlayerStatistics s " +
                "ON s.PlayerId = p.Id WHERE p.Id = @playerId;";
            
            return (await connection.QueryAsync<Player, PlayerStatistic, Player>(sql, (p, s) => 
            {
                p.Statistic = s;
                return p;
            }, new { playerId }, splitOn: "PlayerId")).FirstOrDefault();
        }
    }
}
