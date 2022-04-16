using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UnturnedStrikeAPI;
using UnturnedStrikeAPI.Constants;
using UnturnedStrikeWebBlazor.Shared.Components.Modals;

namespace UnturnedStrikeWebBlazor.Pages.Dashboard
{
    [Authorize(Roles = RoleConstants.AdminRoleId)]
    public partial class BoxesPage
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        public List<Box> Boxes { get; set; }

        private BoxModal Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Boxes = await HttpClient.GetFromJsonAsync<List<Box>>("api/boxes");
        }

        private async Task UploadCreateBoxAsync(Box box)
        {
            var response = await HttpClient.PostAsJsonAsync("api/boxes", box);
            if (response.IsSuccessStatusCode)
            {
                Boxes.Add(await response.Content.ReadFromJsonAsync<Box>());
            }
        }

        private async Task UploadUpdateBoxAsync(Box box)
        {
            await HttpClient.PutAsJsonAsync("api/boxes", box);
        }

        private async Task ShowUpdateBoxModalAsync(Box box)
        {
            await Modal.UpdateBoxAsync(box);
        }

        private async Task ShowCreateBoxModalAsync()
        {
            await Modal.CreateBoxAsync();
        }
    }
}
