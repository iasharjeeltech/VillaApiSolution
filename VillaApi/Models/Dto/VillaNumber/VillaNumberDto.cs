using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VillaApi.Models.Dto.VillaNumber
{
    public class VillaNumberDto
    {
        public int VillaNo { get; set; }
        public string SpeacialDetails { get; set; }
        public DateTime? CreadteDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
