using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories.Sql
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly SqlConnection connection;

        public TransactionsRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<Transaction>> GetPlayerTransactionsAsync(string steamId)
        {
            const string sql = "SELECT * FROM dbo.Transactions WHERE PlayerId = @steamId;";
            return await connection.QueryAsync<Transaction>(sql, new { steamId });            
        }

        public async Task<Transaction> LogTransactionAsync(Transaction purchase)
        {
            const string sql = "INSERT INTO dbo.Transactions (PlayerId, Email, TransactionId, Gross, Fee, Currency) " +
                "VALUES (@PlayerId, @Email, @TransactionId, @Gross, @Fee, @Currency);" +
                "SELECT * FROM dbo.Transactions WHERE Id = SCOPE_IDENTITY();";
            return await connection.QuerySingleAsync<Transaction>(sql, purchase);
        }
    }
}
