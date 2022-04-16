using System.Collections.Generic;

namespace UnturnedStrike.Plugin.Models
{
    public class WeaponCategoryData
    {
        public string CategoryName { get; set; }
        public List<JsonWeaponData> Weapons { get; set; }
    }
}
