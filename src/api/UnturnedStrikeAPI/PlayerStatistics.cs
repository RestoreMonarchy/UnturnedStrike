using System.Text.Json.Serialization;
using UnturnedStrikeAPI.Enumerators;

namespace UnturnedStrikeAPI
{
    public class PlayerStatistic
    {
        public int TotalKills { get; set; }
        public int TotalDeaths { get; set; }
        public int TotalScore { get; set; }
        public int TotalBombsPlanted { get; set; }
        public int TotalBombsDefused { get; set; }
        public int TotalHostagesRescued { get; set; }
        public int GamesWon { get; set; }
        public int GamesLost { get; set; }
        public int BestKills { get; set; }
        public int BestScore { get; set; }
        public int BestKD { get; set; }
        public decimal KD { get; set; }

        [JsonIgnore]
        public int GamesPlayed => GamesWon + GamesLost;
        [JsonIgnore]
        public decimal KillsPerDeath => TotalDeaths == 0 ? TotalKills : TotalKills / (decimal)TotalDeaths;
        [JsonIgnore]
        public decimal KillsPerGame => GamesPlayed == 0 ? TotalKills : TotalKills / (decimal)GamesPlayed;
        [JsonIgnore]
        public decimal DeathsPerGame => GamesPlayed == 0 ? TotalDeaths: TotalDeaths / (decimal)GamesPlayed;
        [JsonIgnore]
        public decimal ScorePerGame => GamesPlayed == 0 ? TotalScore : TotalScore / (decimal)GamesPlayed;

        public string GetPlayerBest(EStatisticsCategory bestPlayerType)
        {
            switch (bestPlayerType)
            {
                case EStatisticsCategory.GamesWon:
                    return $"Won {GamesWon} Games";
                case EStatisticsCategory.TotalKills:
                    return $"Killed {TotalKills} Players";
                case EStatisticsCategory.TotalBombsDefused:
                    return $"Defused {TotalBombsDefused} Bombs";
                case EStatisticsCategory.TotalBombsPlanted:
                    return $"Planted {TotalBombsPlanted} Bombs";
                case EStatisticsCategory.TotalHostagesRescued:
                    return $"Rescued {TotalHostagesRescued} Hostages";
            }
            return string.Empty;
        }
    }
}
