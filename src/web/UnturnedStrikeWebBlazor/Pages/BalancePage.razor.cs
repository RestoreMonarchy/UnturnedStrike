using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
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
    [Authorize(Roles = "Admin")]
    public partial class BalancePage
    {
        [Inject]
        private UserService UserService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private HttpClient HttpClient { get; set; }

        public int Amount { get; set; } = 10;
        public IEnumerable<Transaction> Transactions { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Transactions = await HttpClient.GetFromJsonAsync<Transaction[]>("api/transactions/me");
        }

        private void AddFunds()
        {
            NavigationManager.NavigateTo($"api/transactions?amount={Amount}", true);
        }
    }
}
