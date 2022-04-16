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
    public partial class Profile
    {
        [Parameter]
        public string PlayerId { get; set; }

        [Inject]
        private HttpClient HttpClient { get; set; }

        public Player Player { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Player = await HttpClient.GetFromJsonAsync<Player>($"api/playerstatistics/{PlayerId}");
        }
    }
}
