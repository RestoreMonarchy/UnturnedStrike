using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UnturnedStrikeAPI
{
    public class PlayerWeaponSkin
    {
        public int Id { get; set; }
        [Required]
        public string PlayerId { get; set; }
        [Required]
        public int WeaponSkinId { get; set; }
        public bool IsEquiped { get; set; }
        public DateTime CreateDate { get; set; }

        public Player Player { get; set; }
        public WeaponSkin WeaponSkin { get; set; }
    }
}
