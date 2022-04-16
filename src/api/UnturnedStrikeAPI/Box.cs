using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace UnturnedStrikeAPI
{
    public class Box
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        public int? ImageFileId { get; set; }
        [Required]
        public decimal Price { get; set; }
        public bool IsEnabled { get; set; }

        public List<BoxWeaponSkin> WeaponSkins { get; set; }

        [JsonIgnore]
        public string ImageUrl => $"api/files/{ImageFileId}";
    }
}
