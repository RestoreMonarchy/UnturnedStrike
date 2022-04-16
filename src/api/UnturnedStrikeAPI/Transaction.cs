using System;
using System.Collections.Generic;
using System.Text;

namespace UnturnedStrikeAPI
{
    public class Transaction
    {
        public int Id { get; set; }
        public string PlayerId { get; set; }
        public string Email { get; set; }
        public string TransactionId { get; set; }
        public decimal Gross { get; set; }
        public decimal Fee { get; set; }
        public string Currency { get; set; } 
        public DateTime CreateDate { get; set; }

        public virtual Player Player { get; set; }
    }
}
