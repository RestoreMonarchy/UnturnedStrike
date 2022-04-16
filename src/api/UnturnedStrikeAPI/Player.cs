using System;

namespace UnturnedStrikeAPI
{
    public class Player
    {
        public string Id { get; set; }
        public string SteamName { get; set; }
        public string SteamIconUrl { get; set; }
        public string Country { get; set; }
        public DateTime LastActivity { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsVIP { get; set; }
        public decimal Balance { get; set; }

        public string IP { get; set; }
        public PlayerStatistic Statistic { get; set; }
    }
}
