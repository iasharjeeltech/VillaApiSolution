﻿using System.ComponentModel.DataAnnotations;

namespace VillaApi.Models.Dto
{
    public class VillaNumberCreateDto
    {
        [Required]
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set; }

        [Required]
        public int VillaId { get; set; }
    }
}
