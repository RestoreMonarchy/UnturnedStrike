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
    public partial class WeaponSkinsPage
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        public List<WeaponSkin> WeaponSkins { get; set; }

        private WeaponSkinModal Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            WeaponSkins = await HttpClient.GetFromJsonAsync<List<WeaponSkin>>("api/weaponskins");
        }

        private async Task UploadCreateWeaponSkinAsync(WeaponSkin weaponSkin)
        {
            var response = await HttpClient.PostAsJsonAsync("api/weaponskins", weaponSkin);
            if (response.IsSuccessStatusCode)
            {
                WeaponSkins.Add(await response.Content.ReadFromJsonAsync<WeaponSkin>());
            }
        }

        private async Task UploadUpdateWeaponSkinAsync(WeaponSkin weaponSkin)
        {
            await HttpClient.PutAsJsonAsync("api/weaponskins", weaponSkin);
        }

        private async Task ShowUpdateWeaponSkinModalAsync(WeaponSkin weaponSkin)
        {
            await Modal.UpdateWeaponSkinAsync(weaponSkin);
        }

        private async Task ShowCreateWeaponSkinModalAsync()
        {
            await Modal.CreateWeaponSkinAsync();
        }
    }
}
