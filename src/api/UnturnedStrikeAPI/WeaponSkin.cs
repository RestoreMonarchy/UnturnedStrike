using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace UnturnedStrikeAPI
{
    public class WeaponSkin
    {
        public int Id { get; set; }
        [Required]
        public int WeaponId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        public int? ImageFileId { get; set; }
        [Required]
        public string Rarity { get; set; }

        [JsonIgnore]
        public string ImageUrl => $"api/files/{ImageFileId}";

        public Weapon Weapon { get; set; }
    }
}
