using System;
using System.Text.Json.Serialization;
using UnturnedStrikeAPI.Enumerators;

namespace UnturnedStrikeAPI.Params
{
    public class PlayerStatisticsParams
    {
        public PlayerStatisticsParams() { }

        public PlayerStatisticsParams(DateTime dateFrom, DateTime dateTo, EStatisticsCategory category, int top = 10)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
            Category = category;
            Top = top;
        }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public EStatisticsCategory Category { get; set; }        
        public int Top { get; set; }

        [JsonIgnore]
        public string OrderBy => Category.ToString();
    }
}
