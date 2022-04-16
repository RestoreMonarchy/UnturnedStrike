using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UnturnedStrikeAPI;
using UnturnedStrikeAPI.Enumerators;
using UnturnedStrikeAPI.Params;

namespace UnturnedStrikeWebBlazor.Pages
{
    public partial class Stats
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        private List<Player> Players { get; set; }
        private EStatisticsCategory CurrentCategory { get; set; }
        private EStatisticsPeriod CurrentPeriod { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await ChangePeriodAsync(EStatisticsPeriod.Last30Days);
            await ChangeCategoryAsync(EStatisticsCategory.TotalKills);
        }

        private DateTime dateFrom;
        private readonly DateTime dateTo = DateTime.UtcNow.AddDays(1);

        public async Task ChangePeriodAsync(EStatisticsPeriod period)
        {
            switch (period)
            {
                case EStatisticsPeriod.Last30Days:
                    dateFrom = DateTime.UtcNow.AddDays(-30);
                    break;
                case EStatisticsPeriod.Last7Days:
                    dateFrom = DateTime.UtcNow.AddDays(-7);
                    break;
                case EStatisticsPeriod.Today:
                    dateFrom = DateTime.UtcNow;
                    break;
                case EStatisticsPeriod.Overall:
                    dateFrom = new DateTime(2020, 1, 1);
                    break;
            }
            CurrentPeriod = period;
            await ChangeCategoryAsync(CurrentCategory);
        }

        public async Task ChangeCategoryAsync(EStatisticsCategory category)
        {
            var response = await HttpClient.PostAsJsonAsync($"api/playerstatistics",
                new PlayerStatisticsParams(dateFrom, dateTo, category));
            if (response.IsSuccessStatusCode)
            {
                Players = await response.Content.ReadFromJsonAsync<List<Player>>();
                CurrentCategory = category;
            }
        }

        private string CurrentPeriodString
        {
            get
            {
                switch (CurrentPeriod)
                {
                    case EStatisticsPeriod.Last30Days:
                        return "30 days";
                    case EStatisticsPeriod.Last7Days:
                        return "7 days";
                    case EStatisticsPeriod.Today:
                        return "Today";
                    case EStatisticsPeriod.Overall:
                        return "Overall";
                }
                return null;
            }
        }

        private string GetPlayerSteamIconUrl(int i) => Players.ElementAtOrDefault(i)?.SteamIconUrl ?? string.Empty;
        private string GetPlayerCountry(int i) => Players.ElementAtOrDefault(i)?.Country?.ToLower() ?? "unkown";
        private string GetPlayerSteamName(int i) => Players.ElementAtOrDefault(i)?.SteamName ?? string.Empty;
        private string GetPlayerId(int i) => Players.ElementAtOrDefault(i)?.Id ?? string.Empty;
        private string GetPlayerBest(int i) => Players.ElementAtOrDefault(i)?.Statistic.GetPlayerBest(CurrentCategory) ?? string.Empty;
    }
}
