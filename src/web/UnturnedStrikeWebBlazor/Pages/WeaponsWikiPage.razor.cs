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
    public partial class WeaponsWikiPage
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        private WeaponPreviewModal WeaponPreviewModal { get; set; }

        public IEnumerable<Weapon> Weapons { get; private set; }
        public IEnumerable<IGrouping<string, Weapon>> WeaponCategories => Weapons.GroupBy(x => x.Category);

        protected override async Task OnInitializedAsync()
        {
            Weapons = await HttpClient.GetFromJsonAsync<Weapon[]>("api/weapons");
        }

        public async Task PreviewWeapon(Weapon weapon)
        {
            await WeaponPreviewModal.OpenAsync(weapon);
        }
    }
}
