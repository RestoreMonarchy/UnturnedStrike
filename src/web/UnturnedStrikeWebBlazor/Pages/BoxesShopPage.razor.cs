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
    public partial class BoxesShopPage
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        public IEnumerable<Box> Boxes { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Boxes = await HttpClient.GetFromJsonAsync<Box[]>("api/boxes");
        }
    }
}
