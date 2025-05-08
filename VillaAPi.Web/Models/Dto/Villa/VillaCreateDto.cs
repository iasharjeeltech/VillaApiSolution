using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VillaApi.Web.Models.Dto.Villa
{
    public class VillaCreateDto
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
