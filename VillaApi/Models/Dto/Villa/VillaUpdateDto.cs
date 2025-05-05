using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VillaApi.Models.Dto.Villa
{
    public class VillaUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Name { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public int SqFt { get; set; }
        [Required]
        public int Occupancy { get; set; }
        [Required]
        public double Rate { get; set; }
        public string Amenity { get; set; }
    }
}
