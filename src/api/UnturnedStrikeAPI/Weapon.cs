using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace UnturnedStrikeAPI
{
    public class Weapon
    {
        public int Id { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        public int? ImageFileId { get; set; }
        [Required]
        [StringLength(255)]
        public string Category { get; set; }
        [Required]
        public string Team { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Price { get; set; }
        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal KillRewardMultiplier { get; set; }
        public int? MagazineId { get; set; }
        public int MagazineAmount { get; set; }
        [StringLength(255)]
        public string IconUnicode { get; set; }
        [Required]
        public bool IsEnabled { get; set; }

        [JsonIgnore]
        public string ImageUrl => $"api/files/{ImageFileId}";
    }
}
