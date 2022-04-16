using System;
using System.Collections.Generic;
using System.Text;

namespace UnturnedStrikeAPI
{
    public class VIPPurchase
    {
        public VIPPurchase() { }

        public VIPPurchase(string playerId, decimal amount)
        {
            PlayerId = playerId;
            Amount = amount;
        }

        public int Id { get; set; }
        public string PlayerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
