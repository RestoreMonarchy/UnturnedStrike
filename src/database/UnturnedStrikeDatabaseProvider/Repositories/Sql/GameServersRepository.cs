using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories.Sql
{
    public class GameServersRepository : IGameServersRepository
    {
        private readonly SqlConnection connection;

        public GameServersRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<GameServer>> GetServersAsync()
        {
            const string sql = "SELECT * FROM dbo.GameServers;";
            return await connection.QueryAsync<GameServer>(sql);
        }
    }
}
