using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories
{
    public interface ITransactionsRepository : IRepository
    {
        Task<Transaction> LogTransactionAsync(Transaction purchase);
        Task<IEnumerable<Transaction>> GetPlayerTransactionsAsync(string steamId);
    }
}
