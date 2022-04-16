using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories.Sql
{
    public class PlayersRepository : IPlayersRepository
    {
        private readonly SqlConnection connection;

        public PlayersRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task AddToPlayerBalanceAsync(string steamId, decimal amount)
        {
            const string sql = "UPDATE dbo.Players SET Balance = Balance + @amount WHERE Id = @steamId;";
            await connection.ExecuteAsync(sql, new { steamId, amount });
        }

        public async Task CreatePlayerAsync(string steamId)
        {
            const string sql = "INSERT INTO dbo.Players (Id) VALUES (@steamId);";
            await connection.ExecuteAsync(sql, new { steamId });
        }

        public async Task<Player> GetPlayerAsync(string steamId)
        {
            const string sql = "SELECT * FROM dbo.Players WHERE Id = @steamId;";
            return await connection.QuerySingleOrDefaultAsync<Player>(sql, new { steamId });
        }

        public async Task<IEnumerable<Player>> GetPlayersAsync()
        {
            const string sql = "SELECT * FROM dbo.Players ORDER BY CreateDate DESC;";
            return await connection.QueryAsync<Player>(sql);
        }

        public async Task<Player> UpdatePlayerAsync(Player player)
        {
            const string sql = "UPDATE dbo.Players SET SteamName = @SteamName, SteamIconUrl = @SteamIconUrl, " +
                "Country = ISNULL(@Country, Country), LastActivity = GETUTCDATE() WHERE Id = @Id;" +
                "SELECT * FROM dbo.Players WHERE Id = @Id;";
            
            return await connection.QuerySingleAsync<Player>(sql, player);
        }
    }
}
