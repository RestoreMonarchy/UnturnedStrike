using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories.Sql
{
    public class GameSummariesRepository : IGameSummariesRepository
    {
        private readonly SqlConnection connection;

        public GameSummariesRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<GameSummary> CreateGameSummary(GameSummary gameSummary)
        {
            const string sql = "INSERT INTO dbo.GameSummaries (Map, ServerGroup, GameType) VALUES (@Map, @ServerGroup, @GameType); " +
                "SELECT * FROM dbo.GameSummaries WHERE Id = SCOPE_IDENTITY();";

            return await connection.QuerySingleAsync<GameSummary>(sql, gameSummary);
        }

        public async Task UpdateGameSummary(GameSummary gameSummary)
        {
            const string sql = "UPDATE dbo.GameSummaries SET TerroristScore = @TerroristScore, " +
                "CounterTerroristScore = @CounterTerroristScore, WinType = @WinType, IsWinnerTerrorist = @IsWinnerTerrorist, " +
                "FinishDate = GETUTCDATE() WHERE Id = @Id;";
            const string sql2 = "INSERT INTO dbo.PlayerGameSummaries (GameId, PlayerId, IsTerrorist, Kills, Deaths, " +
                "BombsPlanted, BombsDefused, HostagesRescued, MVPs, Score) VALUES (@GameId, @PlayerId, @IsTerrorist, @Kills, @Deaths, " +
                "@BombsPlanted, @BombsDefused, @HostagesRescued, @MVPs, @Score);";

            await connection.ExecuteAsync(sql, gameSummary);
            foreach (var player in gameSummary.PlayerGameSummaries)
            {
                await connection.ExecuteAsync(sql2, player);
            }
        }
    }
}
