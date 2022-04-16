using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UnturnedStrikeAPI;
using UnturnedStrikeWebBlazor.Shared.Components.Modals;

namespace UnturnedStrikeWebBlazor.Pages
{
    [Authorize]
    public partial class InventoryPage
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        public IEnumerable<PlayerWeaponSkin> PlayerWeaponSkins { get; set; }

        public WeaponSkinPreviewModal PreviewModal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PlayerWeaponSkins = await HttpClient.GetFromJsonAsync<List<PlayerWeaponSkin>>("api/weaponskins/inventory");
        }

        public async Task ToggleEquipSkinAsync(PlayerWeaponSkin playerWeaponSkin)
        {
            var response = await HttpClient.PostAsync($"api/weaponskins/equip/{playerWeaponSkin.Id}", null);
            if (response.IsSuccessStatusCode)
            {
                PlayerWeaponSkins = await HttpClient.GetFromJsonAsync<List<PlayerWeaponSkin>>("api/weaponskins/inventory");
            }                
        }

        public async Task PreviewAsync(WeaponSkin weaponSkin)
        {
            await PreviewModal.OpenAsync(weaponSkin);
        }
    }
}
