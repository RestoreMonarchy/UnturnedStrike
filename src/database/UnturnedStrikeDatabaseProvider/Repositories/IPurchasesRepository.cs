using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeDatabaseProvider.Repositories
{
    public interface IPurchasesRepository : IRepository
    {
        Task<VIPPurchase> GetVIPPurchaseAsync(string playerId);
        Task<Tuple<int, VIPPurchase>> PurchaseVIPAsync(VIPPurchase vipPurchase);
    }
}
