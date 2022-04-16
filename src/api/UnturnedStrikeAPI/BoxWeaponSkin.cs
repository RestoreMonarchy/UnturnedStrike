using System;
using System.Collections.Generic;
using System.Text;

namespace UnturnedStrikeAPI
{
    public class BoxWeaponSkin
    {
        public int BoxId { get; set; }
        public int WeaponSkinId { get; set; }
        public int Weight { get; set; }

        public WeaponSkin WeaponSkin { get; set; }
    }
}
