using System;
using System.Collections.Generic;
using System.Text;

namespace UnturnedStrikeAPI
{
    public class PlayerGameSummary
    {
        public int GameId { get; set; }
        public string PlayerId { get; set; }
        public bool IsTerrorist { get; set; }
        public short Kills { get; set; }
        public short Deaths { get; set; }
        public byte BombsPlanted { get; set; }
        public byte BombsDefused { get; set; }
        public byte HostagesRescued { get; set; }
        public byte MVPs { get; set; }
        public decimal KD { get; set; }
        public short Score { get; set; }
    }
}
