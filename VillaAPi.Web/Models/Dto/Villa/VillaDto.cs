using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VillaApi.Web.Models.Dto.Villa
{
    public class VillaDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string ImageUrl { get; set; }
        public int SqFt { get; set; }
        public int Occupancy { get; set; }
        public string Amenity { get; set; }
        [Required]
        public double Rate { get; set; }

    }
}
