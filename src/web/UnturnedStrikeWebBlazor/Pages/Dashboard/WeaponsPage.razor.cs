using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UnturnedStrikeAPI;
using UnturnedStrikeAPI.Constants;
using UnturnedStrikeWebBlazor.Shared.Components.Modals;

namespace UnturnedStrikeWebBlazor.Pages.Dashboard
{
    [Authorize(Roles = RoleConstants.AdminRoleId)]
    public partial class WeaponsPage
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        public List<Weapon> Weapons { get; set; }

        private WeaponModal Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Weapons = await HttpClient.GetFromJsonAsync<List<Weapon>>("api/weapons");
        }

        private async Task UploadCreateWeaponAsync(Weapon weapon)
        {
            var response = await HttpClient.PostAsJsonAsync("api/weapons", weapon);
            if (response.IsSuccessStatusCode)
            {
                Weapons.Add(await response.Content.ReadFromJsonAsync<Weapon>());
            }
        }

        private async Task UploadUpdateWeaponAsync(Weapon weapon)
        {
            await HttpClient.PutAsJsonAsync("api/weapons", weapon);
        }

        private async Task ShowUpdateWeaponModalAsync(Weapon weapon)
        {
            await Modal.UpdateWeaponAsync(weapon);
        }

        private async Task ShowCreateWeaponModalAsync()
        {
            await Modal.CreateWeaponAsync();
        }
    }
}
