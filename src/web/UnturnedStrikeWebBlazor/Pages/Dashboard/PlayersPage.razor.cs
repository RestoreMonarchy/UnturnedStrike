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
    public partial class PlayersPage
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        public IEnumerable<Player> Players { get; set; }

        private IEnumerable<Player> searchPlayers => Players.Where(
            x => string.IsNullOrEmpty(SearchTerm) || x.Id == SearchTerm || 
            x.SteamName != null && x.SteamName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));

        private string SearchTerm { get; set; } = "";


        public ManagePlayerWeaponSkinsModal ManagePlayerWeaponSkinsModal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Players = await HttpClient.GetFromJsonAsync<Player[]>("api/players");
        }

        private async Task OpenGiveSkinModalAsync(Player player)
        {
            await ManagePlayerWeaponSkinsModal.OpenAsync(player);
        }
    }
}
