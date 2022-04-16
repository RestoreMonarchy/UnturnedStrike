using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UnturnedStrikeAPI;
using UnturnedStrikeWebBlazor.Services;

namespace UnturnedStrikeWebBlazor.Pages
{
    public partial class VIP
    {
        [Inject]
        private UserService UserService { get; set; }
        [Inject]
        private HttpClient HttpClient { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private bool canAfford => UserService.Player.Balance >= 10;
        private string btnClass => canAfford ? string.Empty : "disabled";
        private string btnTooltip => canAfford ? string.Empty : "You can't afford";

        public VIPPurchase Purchase { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (UserService.IsAuthenticated)
                Purchase = await HttpClient.GetFromJsonAsync<VIPPurchase>("api/purchases/vip");
        }

        private async Task BuyVIPAsync()
        {
            var response = await HttpClient.PostAsync("api/purchases/vip", null);
            if (response.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo(NavigationManager.Uri, true);
            }            
        }
    }
}
