using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories.Sql
{
    public class PurchasesRepository : IPurchasesRepository
    {
        private readonly SqlConnection connection;

        public PurchasesRepository(SqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<Tuple<int, VIPPurchase>> PurchaseVIPAsync(VIPPurchase vipPurchase)
        {
            var p = new DynamicParameters();
            p.Add("@PlayerId", vipPurchase.PlayerId);
            p.Add("@Amount", vipPurchase.Amount);
            p.Add("@returnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            vipPurchase = await connection.QuerySingleOrDefaultAsync<VIPPurchase>("dbo.PurchaseVIP", p, commandType: CommandType.StoredProcedure);
            return new Tuple<int, VIPPurchase>(p.Get<int>("@returnValue"), vipPurchase);
        }

        public async Task<VIPPurchase> GetVIPPurchaseAsync(string playerId)
        {
            const string sql = "SELECT * FROM dbo.VIPPurchases WHERE PlayerId = @playerId;";
            return await connection.QueryFirstOrDefaultAsync<VIPPurchase>(sql, new { playerId });
        }
    }
}
