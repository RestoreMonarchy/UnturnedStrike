using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UnturnedStrikeAPI;

namespace UnturnedStrikeWebBlazor.Pages
{
    public partial class Index
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        private IEnumerable<GameServer> GameServers { get; set; }

        protected override async Task OnInitializedAsync()
        {
            GameServers = await HttpClient.GetFromJsonAsync<IEnumerable<GameServer>>("api/gameservers");
        }
    }
}