using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UnturnedStrikeAPI
{
    public class GameSummary
    {
        public int Id { get; set; }
        public string ServerGroup { get; set; }
        public string Map { get; set; }
        public string GameType { get; set; }
        public byte TerroristScore { get; set; }
        public byte CounterTerroristScore { get; set; }
        public string WinType { get; set; }
        public bool IsWinnerTerrorist { get; set; }
        public bool IsFinished { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }

        [JsonIgnore]
        public TimeSpan Duration => (FinishDate ?? DateTime.UtcNow) - StartDate;

        public virtual List<PlayerGameSummary> PlayerGameSummaries { get; set; }
    }
}
